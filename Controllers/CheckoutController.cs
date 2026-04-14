using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _66014444_Project.Models;
using _66014444_Project.Services;
using _66014444_Project.ViewModels.Checkout;

namespace _66014444_Project.Controllers;

[Authorize(Roles = "customer")]
public class CheckoutController : Controller
{
    private readonly _402block2Context _db;

    public CheckoutController(_402block2Context db)
    {
        _db = db;
    }

    public IActionResult Checkout(int? selectedPromotionId = null, int pointsToUse = 0)
    {
        var customerIdClaim = User.FindFirst("customer_id")?.Value;
        if (!int.TryParse(customerIdClaim, out var customerId))
        {
            return Challenge();
        }

        var customer = _db.Customers
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Book)
            .FirstOrDefault(c => c.CustomerId == customerId);

        if (customer is null)
        {
            return NotFound();
        }

        var cartItems = customer.CartItems
            .OrderByDescending(item => item.AddedAt)
            .ToList();

        var promotions = _db.Promotions
            .Include(p => p.PromotionRules)
            .Where(p => p.IsActive == true &&
                        p.StartAt <= DateTime.Now &&
                        (p.EndAt == null || p.EndAt >= DateTime.Now))
            .OrderByDescending(p => p.IsAutoApply)
            .ThenBy(p => p.PromotionName)
            .ToList();

        var pricing = CheckoutPricingCalculator.Calculate(
            cartItems,
            promotions,
            customer.CurrentPoints,
            selectedPromotionId,
            pointsToUse);

        var model = new CheckoutViewModel
        {
            ReceiverName = customer.ReceiverName,
            ReceiverPhone = customer.ReceiverPhone,
            AddressLine1 = customer.AddressLine1,
            AddressLine2 = customer.AddressLine2,
            Subdistrict = customer.Subdistrict,
            District = customer.District,
            Province = customer.Province,
            PostalCode = customer.PostalCode,
            AvailablePoints = customer.CurrentPoints,
            SelectedPromotionId = pricing.SelectedPromotionId,
            AppliedPromotionName = pricing.SelectedPromotionName,
            PointsToUse = pricing.PointsToUse,
            PointsDiscountAmount = pricing.PointsDiscountAmount,
            MaxPointsToUse = pricing.MaxPointsToUse,
            SubtotalAmount = pricing.SubtotalAmount,
            ConditionDiscountAmount = pricing.ConditionDiscountAmount,
            PromotionDiscountAmount = pricing.PromotionDiscountAmount,
            AutoPromotionDiscountAmount = pricing.SameSeriesDiscountAmount,
            SelectedPromotionDiscountAmount = pricing.SelectedPromotionDiscountAmount,
            ShippingFee = pricing.ShippingFee,
            TotalAmount = pricing.TotalAmount,
            PointsToEarn = pricing.PointsToEarn,
            Items = cartItems
                .Select(item =>
                {
                    var conditionDiscountPct = ConditionDiscountHelper.ResolvePercent(item.Book.ConditionCode, item.Book.ConditionDiscountPct);
                    var discountAmount = item.UnitPrice * (conditionDiscountPct / 100m);
                    return new CheckoutItemViewModel
                    {
                        BookId = item.BookId,
                        Title = item.Book.Title,
                        SeriesName = item.Book.SeriesName,
                        ConditionCode = item.Book.ConditionCode,
                        UnitPrice = item.UnitPrice,
                        ConditionDiscountPct = conditionDiscountPct,
                        DiscountAmount = discountAmount,
                        NetAmount = item.UnitPrice - discountAmount,
                        ImageUrl = item.Book.ImageUrl
                    };
                })
                .ToList(),
            AutomaticPromotions =
            [
                new CheckoutAutomaticPromotionViewModel
                {
                    Title = "ส่วนลดตามสภาพหนังสือ",
                    Description = "ระบบคำนวณให้อัตโนมัติจากผลการประเมินหนังสือแต่ละเล่ม",
                    DiscountAmount = pricing.ConditionDiscountAmount,
                    IsApplied = pricing.ConditionDiscountAmount > 0
                },
                new CheckoutAutomaticPromotionViewModel
                {
                    Title = "ซื้อครบ 500 บาท ส่งฟรี",
                    Description = "หากยอดสุทธิหลังหักส่วนลดถึง 500 บาท ระบบจะตัดค่าจัดส่งให้อัตโนมัติ",
                    DiscountAmount = pricing.FreeShippingApplied ? 50m : 0m,
                    IsApplied = pricing.FreeShippingApplied
                },
                new CheckoutAutomaticPromotionViewModel
                {
                    Title = "นิยายชุดเดียวกันลดเพิ่ม 50 บาท",
                    Description = "ลด 50 บาทต่อชุดนิยายที่มีในตะกร้าอย่างน้อย 2 เล่ม",
                    DiscountAmount = pricing.SameSeriesDiscountAmount,
                    IsApplied = pricing.SameSeriesDiscountAmount > 0
                }
            ],
            Promotions = pricing.EligiblePromotions
                .Select(p => new CheckoutPromotionViewModel
                {
                    PromotionId = p.PromotionId,
                    PromotionCode = p.PromotionCode,
                    PromotionName = p.PromotionName,
                    Description = p.Description,
                    PromotionType = "flash_sale",
                    DiscountType = p.DiscountType,
                    DiscountValue = p.DiscountValue,
                    MinOrderAmount = p.MinOrderAmount,
                    MinItemQty = p.MinItemQty,
                    TargetSeriesName = p.TargetSeriesName,
                    RuleLabel = p.RuleLabel,
                    IsEligible = p.IsEligible,
                    CalculatedDiscountAmount = p.DiscountAmount,
                    IsSelected = p.PromotionId == pricing.SelectedPromotionId
                })
                .ToList()
        };

        return View(model);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _66014444_Project.Models;
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

    public IActionResult Checkout()
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
            .Where(p => p.IsActive == true && p.StartAt <= DateTime.Now && (p.EndAt == null || p.EndAt >= DateTime.Now))
            .OrderByDescending(p => p.IsAutoApply)
            .ThenBy(p => p.PromotionName)
            .ToList();

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
            Items = cartItems
                .Select(item =>
                {
                    var discountAmount = item.UnitPrice * (item.Book.ConditionDiscountPct / 100m);
                    return new CheckoutItemViewModel
                    {
                        BookId = item.BookId,
                        Title = item.Book.Title,
                        SeriesName = item.Book.SeriesName,
                        ConditionCode = item.Book.ConditionCode,
                        UnitPrice = item.UnitPrice,
                        ConditionDiscountPct = item.Book.ConditionDiscountPct,
                        DiscountAmount = discountAmount,
                        NetAmount = item.UnitPrice - discountAmount,
                        ImageUrl = item.Book.ImageUrl
                    };
                })
                .ToList(),
            Promotions = promotions
                .Select(p => new CheckoutPromotionViewModel
                {
                    PromotionId = p.PromotionId,
                    PromotionCode = p.PromotionCode,
                    PromotionName = p.PromotionName,
                    Description = p.Description,
                    PromotionType = p.PromotionType,
                    DiscountType = p.DiscountType,
                    DiscountValue = p.DiscountValue,
                    MinOrderAmount = p.MinOrderAmount
                })
                .ToList()
        };

        model.SubtotalAmount = model.Items.Sum(item => item.UnitPrice);
        model.ConditionDiscountAmount = model.Items.Sum(item => item.DiscountAmount);
        model.PromotionDiscountAmount = 0m;
        model.ShippingFee = model.Items.Count > 0 ? 50m : 0m;
        var subtotalAfterCondition = model.Items.Sum(item => item.NetAmount);
        model.MaxPointsToUse = Math.Min(model.AvailablePoints, (int)Math.Floor(subtotalAfterCondition));
        model.TotalAmount = subtotalAfterCondition + model.ShippingFee;
        model.PointsToEarn = (int)Math.Floor(model.TotalAmount / 100m);

        return View(model);
    }
}

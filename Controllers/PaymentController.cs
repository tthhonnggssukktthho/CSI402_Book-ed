using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _66014444_Project.Models;
using _66014444_Project.Services;
using _66014444_Project.ViewModels.Payment;

namespace _66014444_Project.Controllers;

[Authorize(Roles = "customer")]
public class PaymentController : Controller
{
    private readonly _402block2Context _db;
    private readonly IWebHostEnvironment _environment;

    public PaymentController(_402block2Context db, IWebHostEnvironment environment)
    {
        _db = db;
        _environment = environment;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(Upload));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(int? selectedPromotionId, int pointsToUse = 0)
    {
        var customerId = GetCurrentCustomerId();
        if (!customerId.HasValue)
        {
            return Challenge();
        }

        var order = EnsurePendingOrder(customerId.Value, selectedPromotionId, pointsToUse);
        if (order is null)
        {
            TempData["CartSuccess"] = "ยังไม่มีสินค้าในตะกร้าสำหรับสร้างคำสั่งซื้อ";
            return RedirectToAction("CartIndex", "Cart");
        }

        return RedirectToAction(nameof(Upload), new { orderId = order.OrderId });
    }

    public IActionResult Upload(int? orderId = null)
    {
        var customerId = GetCurrentCustomerId();
        if (!customerId.HasValue)
        {
            return Challenge();
        }

        Order? order;
        if (orderId.HasValue)
        {
            order = _db.Orders.FirstOrDefault(o => o.OrderId == orderId.Value && o.CustomerId == customerId.Value);
        }
        else
        {
            order = EnsurePendingOrder(customerId.Value, null, 0);
        }

        if (order is null)
        {
            TempData["CartSuccess"] = "ยังไม่มีสินค้าในตะกร้าสำหรับสร้างคำสั่งซื้อ";
            return RedirectToAction("CartIndex", "Cart");
        }

        var model = new PaymentUploadViewModel
        {
            OrderId = order.OrderId,
            OrderNo = order.OrderNo,
            TotalAmount = order.TotalAmount,
            PaymentDueAt = order.PaymentDueAt,
            TransferAmount = order.TotalAmount,
            TransferDatetime = DateTime.Now
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UploadLater(int orderId)
    {
        var customerId = GetCurrentCustomerId();
        if (!customerId.HasValue)
        {
            return Challenge();
        }

        var order = _db.Orders.FirstOrDefault(o => o.OrderId == orderId && o.CustomerId == customerId.Value);
        if (order is null)
        {
            return NotFound();
        }

        TempData["OrderSuccess"] = "บันทึกคำสั่งซื้อแล้ว คุณสามารถอัปโหลดหลักฐานการชำระเงินภายหลังได้";
        return RedirectToAction("Index", "Order");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(PaymentUploadViewModel model)
    {
        var customerId = GetCurrentCustomerId();
        if (!customerId.HasValue)
        {
            return Challenge();
        }

        var order = _db.Orders
            .Include(o => o.Payments)
            .FirstOrDefault(o => o.OrderId == model.OrderId && o.CustomerId == customerId.Value);

        if (order is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.OrderNo = order.OrderNo;
            model.TotalAmount = order.TotalAmount;
            model.PaymentDueAt = order.PaymentDueAt;
            return View(model);
        }

        var uploadDir = Path.Combine(_environment.WebRootPath, "uploads", "payments");
        Directory.CreateDirectory(uploadDir);

        var fileExtension = Path.GetExtension(model.EvidenceFile!.FileName);
        var fileName = $"{Guid.NewGuid():N}{fileExtension}";
        var filePath = Path.Combine(uploadDir, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await model.EvidenceFile.CopyToAsync(stream);
        }

        var payment = order.Payments.OrderByDescending(p => p.CreatedAt).FirstOrDefault();
        if (payment is null)
        {
            payment = new Payment
            {
                OrderId = order.OrderId,
                PaymentMethod = "bank_transfer",
                PaymentStatus = "submitted",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _db.Payments.Add(payment);
        }

        payment.TransferAmount = model.TransferAmount;
        payment.TransferDatetime = model.TransferDatetime;
        payment.PayerName = model.PayerName;
        payment.EvidenceUrl = $"/uploads/payments/{fileName}";
        payment.EvidenceUploadedAt = DateTime.Now;
        payment.PaymentStatus = "submitted";
        payment.UpdatedAt = DateTime.Now;

        order.PreviousStatus = order.OrderStatus;
        order.OrderStatus = "payment_submitted";
        order.UpdatedAt = DateTime.Now;

        _db.SaveChanges();

        TempData["OrderSuccess"] = "ส่งหลักฐานการชำระเงินเรียบร้อยแล้ว";
        return RedirectToAction("Index", "Order");
    }

    private int? GetCurrentCustomerId()
    {
        var customerIdClaim = User.FindFirst("customer_id")?.Value;
        if (int.TryParse(customerIdClaim, out var customerId))
        {
            return customerId;
        }

        return null;
    }

    private Order? EnsurePendingOrder(int customerId, int? selectedPromotionId, int pointsToUse)
    {
        var existingOrder = _db.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefault(o => o.CustomerId == customerId && o.OrderStatus == "pending_payment");

        if (existingOrder is not null)
        {
            return existingOrder;
        }

        var customer = _db.Customers
            .Include(c => c.User)
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Book)
            .FirstOrDefault(c => c.CustomerId == customerId);

        if (customer is null || customer.CartItems.Count == 0)
        {
            return null;
        }

        var now = DateTime.Now;
        var cartItems = customer.CartItems.ToList();
        var promotions = _db.Promotions
            .Include(p => p.PromotionRules)
            .Where(p => p.IsActive == true &&
                        p.StartAt <= now &&
                        (p.EndAt == null || p.EndAt >= now))
            .ToList();

        var pricing = CheckoutPricingCalculator.Calculate(
            cartItems,
            promotions,
            customer.CurrentPoints,
            selectedPromotionId,
            pointsToUse);

        var order = new Order
        {
            OrderNo = $"ORD{now:yyyyMMddHHmmss}{customerId:D4}",
            CustomerId = customer.CustomerId,
            SnapReceiverName = customer.ReceiverName ?? customer.DisplayName,
            SnapReceiverPhone = customer.ReceiverPhone ?? customer.User.PhoneNumber ?? "-",
            SnapAddressLine1 = customer.AddressLine1 ?? "-",
            SnapAddressLine2 = customer.AddressLine2,
            SnapSubdistrict = customer.Subdistrict ?? "-",
            SnapDistrict = customer.District ?? "-",
            SnapProvince = customer.Province ?? "-",
            SnapPostalCode = customer.PostalCode ?? "-",
            OrderStatus = "pending_payment",
            PreviousStatus = null,
            SubtotalAmount = pricing.SubtotalAmount,
            ConditionDiscountAmount = pricing.ConditionDiscountAmount,
            PromotionDiscountAmount = pricing.PromotionDiscountAmount,
            PointsDiscountAmount = pricing.PointsDiscountAmount,
            ShippingFee = pricing.ShippingFee,
            TotalAmount = pricing.TotalAmount,
            PointsEarned = pricing.PointsToEarn,
            PointsUsed = pricing.PointsToUse,
            PromotionId = pricing.SelectedPromotionId,
            PaymentDueAt = now.AddHours(24),
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.Orders.Add(order);
        _db.SaveChanges();

        foreach (var cartItem in cartItems)
        {
            var conditionDiscountPct = ConditionDiscountHelper.ResolvePercent(cartItem.Book.ConditionCode, cartItem.Book.ConditionDiscountPct);
            var discountAmount = cartItem.UnitPrice * (conditionDiscountPct / 100m);
            _db.OrderItems.Add(new OrderItem
            {
                OrderId = order.OrderId,
                BookId = cartItem.BookId,
                SellerCustomerId = cartItem.Book.SellerCustomerId,
                BookTitleSnapshot = cartItem.Book.Title,
                SeriesNameSnapshot = cartItem.Book.SeriesName,
                ConditionCodeSnap = cartItem.Book.ConditionCode,
                UnitPrice = cartItem.UnitPrice,
                DiscountAmount = discountAmount,
                NetAmount = cartItem.UnitPrice - discountAmount,
                CreatedAt = now
            });
        }

        _db.Payments.Add(new Payment
        {
            OrderId = order.OrderId,
            PaymentMethod = "bank_transfer",
            PaymentStatus = "pending",
            TransferAmount = 0m,
            CreatedAt = now,
            UpdatedAt = now
        });

        _db.CartItems.RemoveRange(cartItems);
        _db.SaveChanges();

        return order;
    }
}

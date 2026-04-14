using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _66014444_Project.Models;
using _66014444_Project.ViewModels.Finance;
using _66014444_Project.ViewModels.Order;

namespace _66014444_Project.Controllers;

[Authorize(Roles = "admin,finance")]
public class FinanceController : Controller
{
    private readonly _402block2Context _db;

    public FinanceController(_402block2Context db)
    {
        _db = db;
    }

    public IActionResult Queue()
    {
        var model = new FinanceQueueViewModel
        {
            Payments = _db.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.Customer)
                .OrderByDescending(p => p.PaymentStatus == "submitted")
                .ThenByDescending(p => p.EvidenceUploadedAt ?? p.CreatedAt)
                .Select(p => new FinanceQueueItemViewModel
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    OrderNo = p.Order.OrderNo,
                    CustomerDisplayName = p.Order.Customer.DisplayName,
                    TotalAmount = p.Order.TotalAmount,
                    TransferAmount = p.TransferAmount,
                    PaymentStatus = p.PaymentStatus,
                    OrderStatus = p.Order.OrderStatus,
                    EvidenceUploadedAt = p.EvidenceUploadedAt,
                    CreatedAt = p.CreatedAt,
                    HasEvidence = !string.IsNullOrWhiteSpace(p.EvidenceUrl)
                })
                .ToList()
        };

        return View(model);
    }

    public IActionResult OrderDetail(int id)
    {
        var payment = LoadPayment(id);
        if (payment is null)
        {
            return NotFound();
        }

        return View(BuildDetailViewModel(payment));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ApprovePayment(int paymentId)
    {
        var payment = LoadPayment(paymentId);
        if (payment is null)
        {
            return NotFound();
        }

        if (payment.PaymentStatus is not ("submitted" or "pending"))
        {
            TempData["FinanceError"] = "รายการนี้ไม่อยู่ในสถานะที่อนุมัติได้";
            return RedirectToAction(nameof(OrderDetail), new { id = paymentId });
        }

        var employeeId = GetCurrentEmployeeId();
        var now = DateTime.Now;

        payment.PaymentStatus = "approved";
        payment.RejectReason = null;
        payment.VerifiedByEmployeeId = employeeId;
        payment.VerifiedAt = now;
        payment.UpdatedAt = now;

        payment.Order.PreviousStatus = payment.Order.OrderStatus;
        payment.Order.OrderStatus = "payment_approved";
        payment.Order.UpdatedAt = now;

        var purchasedBookIds = payment.Order.OrderItems
            .Select(item => item.BookId)
            .Distinct()
            .ToList();

        var purchasedBooks = _db.Books
            .Where(book => purchasedBookIds.Contains(book.BookId))
            .ToList();

        foreach (var book in purchasedBooks)
        {
            book.SaleStatus = "sold";
            book.UpdatedAt = now;
        }

        var cartItemsToRemove = _db.CartItems
            .Where(ci => purchasedBookIds.Contains(ci.BookId))
            .ToList();
        if (cartItemsToRemove.Count > 0)
        {
            _db.CartItems.RemoveRange(cartItemsToRemove);
        }

        if (payment.Order.Shipment is null)
        {
            _db.Shipments.Add(new Shipment
            {
                OrderId = payment.OrderId,
                ShipmentStatus = "waiting_to_pack",
                CreatedAt = now,
                UpdatedAt = now
            });
        }
        else
        {
            payment.Order.Shipment.ShipmentStatus = "waiting_to_pack";
            payment.Order.Shipment.UpdatedAt = now;
        }

        _db.SaveChanges();
        TempData["FinanceSuccess"] = "อนุมัติการชำระเงินเรียบร้อยแล้ว";
        return RedirectToAction(nameof(OrderDetail), new { id = paymentId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RejectPayment(int paymentId, string? rejectReason)
    {
        var payment = LoadPayment(paymentId);
        if (payment is null)
        {
            return NotFound();
        }

        if (payment.PaymentStatus is not ("submitted" or "pending"))
        {
            TempData["FinanceError"] = "รายการนี้ไม่อยู่ในสถานะที่เปลี่ยนผลการตรวจสอบได้";
            return RedirectToAction(nameof(OrderDetail), new { id = paymentId });
        }

        if (string.IsNullOrWhiteSpace(rejectReason))
        {
            TempData["FinanceError"] = "กรุณาระบุเหตุผลที่ไม่อนุมัติ";
            return RedirectToAction(nameof(OrderDetail), new { id = paymentId });
        }

        var employeeId = GetCurrentEmployeeId();
        var now = DateTime.Now;

        payment.PaymentStatus = "rejected";
        payment.RejectReason = rejectReason.Trim();
        payment.VerifiedByEmployeeId = employeeId;
        payment.VerifiedAt = now;
        payment.UpdatedAt = now;

        payment.Order.PreviousStatus = payment.Order.OrderStatus;
        payment.Order.OrderStatus = "pending_payment";
        payment.Order.UpdatedAt = now;

        var reservedBookIds = payment.Order.OrderItems
            .Select(item => item.BookId)
            .Distinct()
            .ToList();

        var reservedBooks = _db.Books
            .Where(book => reservedBookIds.Contains(book.BookId) && book.SaleStatus == "reserved")
            .ToList();

        foreach (var book in reservedBooks)
        {
            book.SaleStatus = "ready_for_sale";
            book.UpdatedAt = now;
        }

        _db.SaveChanges();
        TempData["FinanceSuccess"] = "บันทึกผลการไม่อนุมัติเรียบร้อยแล้ว";
        return RedirectToAction(nameof(OrderDetail), new { id = paymentId });
    }

    private Payment? LoadPayment(int paymentId)
    {
        return _db.Payments
            .Include(p => p.VerifiedByEmployee)
            .Include(p => p.Order)
            .ThenInclude(o => o.Customer)
            .Include(p => p.Order)
            .ThenInclude(o => o.OrderItems)
            .Include(p => p.Order)
            .ThenInclude(o => o.Shipment)
            .FirstOrDefault(p => p.PaymentId == paymentId);
    }

    private FinancePaymentDetailViewModel BuildDetailViewModel(Payment payment)
    {
        return new FinancePaymentDetailViewModel
        {
            OrderId = payment.OrderId,
            OrderNo = payment.Order.OrderNo,
            CustomerDisplayName = payment.Order.Customer.DisplayName,
            SnapReceiverName = payment.Order.SnapReceiverName,
            SnapReceiverPhone = payment.Order.SnapReceiverPhone,
            SnapAddressLine1 = payment.Order.SnapAddressLine1,
            SnapAddressLine2 = payment.Order.SnapAddressLine2,
            SnapSubdistrict = payment.Order.SnapSubdistrict,
            SnapDistrict = payment.Order.SnapDistrict,
            SnapProvince = payment.Order.SnapProvince,
            SnapPostalCode = payment.Order.SnapPostalCode,
            Items = payment.Order.OrderItems.Select(item => new OrderItemViewModel
            {
                OrderItemId = item.OrderItemId,
                BookTitleSnapshot = item.BookTitleSnapshot,
                SeriesNameSnapshot = item.SeriesNameSnapshot ?? string.Empty,
                ConditionCodeSnap = item.ConditionCodeSnap,
                UnitPrice = item.UnitPrice,
                DiscountAmount = item.DiscountAmount,
                NetAmount = item.NetAmount
            }).ToList(),
            TotalAmount = payment.Order.TotalAmount,
            SubtotalAmount = payment.Order.SubtotalAmount,
            ShippingFee = payment.Order.ShippingFee,
            PaymentId = payment.PaymentId,
            PaymentStatus = payment.PaymentStatus,
            TransferAmount = payment.TransferAmount,
            TransferDatetime = payment.TransferDatetime,
            PayerName = payment.PayerName,
            EvidenceUrl = payment.EvidenceUrl,
            EvidenceUploadedAt = payment.EvidenceUploadedAt,
            OrderStatus = payment.Order.OrderStatus,
            ShipmentStatus = payment.Order.Shipment?.ShipmentStatus ?? "waiting_payment_verification",
            VerifiedByName = payment.VerifiedByEmployee?.FullName,
            VerifiedAt = payment.VerifiedAt,
            RejectReason = payment.RejectReason
        };
    }

    private int? GetCurrentEmployeeId()
    {
        var employeeIdClaim = User.FindFirst("employee_id")?.Value;
        if (int.TryParse(employeeIdClaim, out var employeeId))
        {
            return employeeId;
        }

        return null;
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _66014444_Project.Models;
using _66014444_Project.ViewModels.Order;

namespace _66014444_Project.Controllers;

[Authorize(Roles = "customer")]
public class OrderController : Controller
{
    private readonly _402block2Context _db;

    public OrderController(_402block2Context db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var customerIdClaim = User.FindFirst("customer_id")?.Value;
        if (!int.TryParse(customerIdClaim, out var customerId))
        {
            return Challenge();
        }

        var model = new OrderIndexViewModel
        {
            Orders = _db.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new OrderSummaryViewModel
                {
                    OrderId = o.OrderId,
                    OrderNo = o.OrderNo,
                    CreatedAt = o.CreatedAt,
                    ItemCount = o.OrderItems.Count,
                    TotalAmount = o.TotalAmount,
                    OrderStatus = o.OrderStatus,
                    PaymentDueAt = o.PaymentDueAt,
                    IsExpiringSoon = o.OrderStatus == "pending_payment" && o.PaymentDueAt <= DateTime.Now.AddHours(1)
                })
                .ToList()
        };

        model.TotalPages = 1;
        return View(model);
    }

    public IActionResult Detail(int id)
    {
        var customerIdClaim = User.FindFirst("customer_id")?.Value;
        if (!int.TryParse(customerIdClaim, out var customerId))
        {
            return Challenge();
        }

        var order = _db.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.Payments)
            .Include(o => o.Shipment)
            .FirstOrDefault(o => o.OrderId == id && o.CustomerId == customerId);

        if (order is null)
        {
            return NotFound();
        }

        var payment = order.Payments.OrderByDescending(p => p.CreatedAt).FirstOrDefault();

        var model = new OrderDetailViewModel
        {
            OrderId = order.OrderId,
            OrderNo = order.OrderNo,
            OrderStatus = order.OrderStatus,
            PreviousStatus = order.PreviousStatus ?? "-",
            CreatedAt = order.CreatedAt,
            PaymentDueAt = order.PaymentDueAt,
            CancelledAt = order.CancelledAt,
            CancelReason = order.CancelReason ?? "-",
            SnapReceiverName = order.SnapReceiverName,
            SnapReceiverPhone = order.SnapReceiverPhone,
            SnapAddressLine1 = order.SnapAddressLine1,
            SnapAddressLine2 = order.SnapAddressLine2 ?? string.Empty,
            SnapSubdistrict = order.SnapSubdistrict,
            SnapDistrict = order.SnapDistrict,
            SnapProvince = order.SnapProvince,
            SnapPostalCode = order.SnapPostalCode,
            Items = order.OrderItems.Select(item => new OrderItemViewModel
            {
                OrderItemId = item.OrderItemId,
                BookTitleSnapshot = item.BookTitleSnapshot,
                SeriesNameSnapshot = item.SeriesNameSnapshot ?? string.Empty,
                ConditionCodeSnap = item.ConditionCodeSnap,
                UnitPrice = item.UnitPrice,
                DiscountAmount = item.DiscountAmount,
                NetAmount = item.NetAmount
            }).ToList(),
            SubtotalAmount = order.SubtotalAmount,
            ConditionDiscountAmount = order.ConditionDiscountAmount,
            PromotionDiscountAmount = order.PromotionDiscountAmount,
            PointsDiscountAmount = order.PointsDiscountAmount,
            ShippingFee = order.ShippingFee,
            TotalAmount = order.TotalAmount,
            PointsEarned = order.PointsEarned,
            PointsUsed = order.PointsUsed,
            PaymentStatus = payment?.PaymentStatus ?? "pending",
            TransferAmount = payment?.TransferAmount,
            TransferDatetime = payment?.TransferDatetime,
            PayerName = payment?.PayerName ?? string.Empty,
            EvidenceUrl = payment?.EvidenceUrl ?? string.Empty,
            RejectReason = payment?.RejectReason ?? string.Empty,
            CanUploadPayment = order.OrderStatus == "pending_payment",
            ShipmentStatus = order.Shipment?.ShipmentStatus ?? "waiting_payment_verification",
            CarrierName = order.Shipment?.CarrierName ?? string.Empty,
            TrackingNo = order.Shipment?.TrackingNo ?? string.Empty,
            ShippedAt = order.Shipment?.ShippedAt,
            DeliveredAt = order.Shipment?.DeliveredAt
        };

        return View(model);
    }
}

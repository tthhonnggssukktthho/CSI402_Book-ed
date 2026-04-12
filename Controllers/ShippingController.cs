using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _66014444_Project.Models;
using _66014444_Project.ViewModels.Order;
using _66014444_Project.ViewModels.Shipping;

namespace _66014444_Project.Controllers;

[Authorize(Roles = "admin,shipping")]
public class ShippingController : Controller
{
    private readonly _402block2Context _db;

    public ShippingController(_402block2Context db)
    {
        _db = db;
    }

    public IActionResult Queue()
    {
        var model = new ShippingQueueViewModel
        {
            Shipments = _db.Shipments
                .Include(s => s.Order)
                .ThenInclude(o => o.Customer)
                .Include(s => s.Order)
                .ThenInclude(o => o.OrderItems)
                .OrderBy(s => s.ShipmentStatus == "waiting_to_pack" ? 0 : s.ShipmentStatus == "packed" ? 1 : 2)
                .ThenByDescending(s => s.CreatedAt)
                .Select(s => new ShippingQueueItemViewModel
                {
                    ShipmentId = s.ShipmentId,
                    OrderId = s.OrderId,
                    OrderNo = s.Order.OrderNo,
                    CustomerDisplayName = s.Order.Customer.DisplayName,
                    SnapProvince = s.Order.SnapProvince,
                    ItemCount = s.Order.OrderItems.Count,
                    ShipmentStatus = s.ShipmentStatus,
                    CarrierName = s.CarrierName,
                    TrackingNo = s.TrackingNo,
                    CreatedAt = s.CreatedAt
                })
                .ToList()
        };

        return View(model);
    }

    public IActionResult PackDetail(int id)
    {
        var shipment = LoadShipment(id);
        if (shipment is null)
        {
            return NotFound();
        }

        var model = new ShippingPackDetailViewModel
        {
            ShipmentId = shipment.ShipmentId,
            OrderId = shipment.OrderId,
            OrderNo = shipment.Order.OrderNo,
            ShipmentStatus = shipment.ShipmentStatus,
            SnapReceiverName = shipment.Order.SnapReceiverName,
            SnapReceiverPhone = shipment.Order.SnapReceiverPhone,
            SnapAddressLine1 = shipment.Order.SnapAddressLine1,
            SnapAddressLine2 = shipment.Order.SnapAddressLine2,
            SnapSubdistrict = shipment.Order.SnapSubdistrict,
            SnapDistrict = shipment.Order.SnapDistrict,
            SnapProvince = shipment.Order.SnapProvince,
            SnapPostalCode = shipment.Order.SnapPostalCode,
            CustomerDisplayName = shipment.Order.Customer.DisplayName,
            TotalAmount = shipment.Order.TotalAmount,
            CarrierName = shipment.CarrierName,
            TrackingNo = shipment.TrackingNo,
            Items = shipment.Order.OrderItems.Select(item => new OrderItemViewModel
            {
                OrderItemId = item.OrderItemId,
                BookTitleSnapshot = item.BookTitleSnapshot,
                SeriesNameSnapshot = item.SeriesNameSnapshot ?? string.Empty,
                ConditionCodeSnap = item.ConditionCodeSnap,
                UnitPrice = item.UnitPrice,
                DiscountAmount = item.DiscountAmount,
                NetAmount = item.NetAmount
            }).ToList()
        };

        return View(model);
    }

    public IActionResult UpdateTracking(int id)
    {
        var shipment = LoadShipment(id);
        if (shipment is null)
        {
            return NotFound();
        }

        return View(BuildTrackingViewModel(shipment));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateTracking(ShippingUpdateTrackingViewModel model)
    {
        var shipment = LoadShipment(model.ShipmentId);
        if (shipment is null)
        {
            return NotFound();
        }

        model.CarrierOptions = BuildCarrierOptions();
        model.StatusOptions = BuildStatusOptions();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var now = DateTime.Now;
        var employeeId = GetCurrentEmployeeId();

        shipment.CarrierName = model.CarrierName.Trim();
        shipment.TrackingNo = model.TrackingNo.Trim();
        shipment.ShipmentStatus = model.ShipmentStatus;
        shipment.PackedByEmployeeId = employeeId;
        shipment.UpdatedAt = now;

        if (model.ShipmentStatus is "packed" && shipment.ShippingLabelPrintedAt is null)
        {
            shipment.ShippingLabelPrintedAt = now;
        }

        if (model.ShipmentStatus is "shipped")
        {
            shipment.ShippedAt ??= now;
            shipment.Order.PreviousStatus = shipment.Order.OrderStatus;
            shipment.Order.OrderStatus = "shipped";
            shipment.Order.UpdatedAt = now;
        }
        else if (model.ShipmentStatus is "delivered")
        {
            shipment.ShippedAt ??= now;
            shipment.DeliveredAt ??= now;
            shipment.Order.PreviousStatus = shipment.Order.OrderStatus;
            shipment.Order.OrderStatus = "completed";
            shipment.Order.UpdatedAt = now;
        }
        else
        {
            shipment.Order.PreviousStatus = shipment.Order.OrderStatus;
            shipment.Order.OrderStatus = "payment_approved";
            shipment.Order.UpdatedAt = now;
        }

        _db.SaveChanges();
        TempData["ShippingSuccess"] = "อัปเดตข้อมูลการจัดส่งเรียบร้อยแล้ว";
        return RedirectToAction(nameof(UpdateTracking), new { id = shipment.ShipmentId });
    }

    private Shipment? LoadShipment(int shipmentId)
    {
        return _db.Shipments
            .Include(s => s.PackedByEmployee)
            .Include(s => s.Order)
            .ThenInclude(o => o.Customer)
            .Include(s => s.Order)
            .ThenInclude(o => o.OrderItems)
            .FirstOrDefault(s => s.ShipmentId == shipmentId);
    }

    private ShippingUpdateTrackingViewModel BuildTrackingViewModel(Shipment shipment)
    {
        return new ShippingUpdateTrackingViewModel
        {
            ShipmentId = shipment.ShipmentId,
            OrderNo = shipment.Order.OrderNo,
            CurrentStatus = shipment.ShipmentStatus,
            CarrierName = shipment.CarrierName ?? string.Empty,
            TrackingNo = shipment.TrackingNo ?? string.Empty,
            ShipmentStatus = shipment.ShipmentStatus,
            CarrierOptions = BuildCarrierOptions(),
            StatusOptions = BuildStatusOptions()
        };
    }

    private static List<SelectListItem> BuildCarrierOptions()
    {
        return
        [
            new SelectListItem { Value = "ไปรษณีย์ไทย", Text = "ไปรษณีย์ไทย" },
            new SelectListItem { Value = "Flash Express", Text = "Flash Express" },
            new SelectListItem { Value = "J&T Express", Text = "J&T Express" },
            new SelectListItem { Value = "Kerry Express", Text = "Kerry Express" },
            new SelectListItem { Value = "DHL", Text = "DHL" }
        ];
    }

    private static List<SelectListItem> BuildStatusOptions()
    {
        return
        [
            new SelectListItem { Value = "waiting_to_pack", Text = "รอแพ็ก" },
            new SelectListItem { Value = "packed", Text = "แพ็กแล้ว" },
            new SelectListItem { Value = "shipped", Text = "จัดส่งแล้ว" },
            new SelectListItem { Value = "delivered", Text = "ส่งสำเร็จแล้ว" }
        ];
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

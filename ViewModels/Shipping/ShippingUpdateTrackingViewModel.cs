using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _66014444_Project.ViewModels.Shipping;

public class ShippingUpdateTrackingViewModel
{
    public int ShipmentId { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string CurrentStatus { get; set; } = string.Empty;

    [Required(ErrorMessage = "กรุณาเลือกบริษัทขนส่ง")]
    [StringLength(100)]
    public string CarrierName { get; set; } = string.Empty;

    [Required(ErrorMessage = "กรุณากรอกเลขติดตามพัสดุ")]
    [StringLength(100)]
    public string TrackingNo { get; set; } = string.Empty;

    [Required(ErrorMessage = "กรุณาเลือกสถานะจัดส่ง")]
    public string ShipmentStatus { get; set; } = string.Empty;

    public List<SelectListItem> CarrierOptions { get; set; } = new();
    public List<SelectListItem> StatusOptions { get; set; } = new();
}

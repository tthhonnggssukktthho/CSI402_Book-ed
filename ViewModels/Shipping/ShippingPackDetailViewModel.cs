using _66014444_Project.ViewModels.Order;

namespace _66014444_Project.ViewModels.Shipping;

public class ShippingPackDetailViewModel
{
    public int ShipmentId { get; set; }
    public int OrderId { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string ShipmentStatus { get; set; } = string.Empty;
    public string CustomerDisplayName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string? CarrierName { get; set; }
    public string? TrackingNo { get; set; }

    public string SnapReceiverName { get; set; } = string.Empty;
    public string SnapReceiverPhone { get; set; } = string.Empty;
    public string SnapAddressLine1 { get; set; } = string.Empty;
    public string? SnapAddressLine2 { get; set; }
    public string SnapSubdistrict { get; set; } = string.Empty;
    public string SnapDistrict { get; set; } = string.Empty;
    public string SnapProvince { get; set; } = string.Empty;
    public string SnapPostalCode { get; set; } = string.Empty;

    public List<OrderItemViewModel> Items { get; set; } = new();
}

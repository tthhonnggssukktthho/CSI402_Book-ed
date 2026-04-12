namespace _66014444_Project.ViewModels.Shipping;

public class ShippingQueueViewModel
{
    public List<ShippingQueueItemViewModel> Shipments { get; set; } = new();
    public string FilterStatus { get; set; } = "all";
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class ShippingQueueItemViewModel
{
    public int ShipmentId { get; set; }
    public int OrderId { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string CustomerDisplayName { get; set; } = string.Empty;
    public string SnapProvince { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public string ShipmentStatus { get; set; } = string.Empty;
    public string? CarrierName { get; set; }
    public string? TrackingNo { get; set; }
    public DateTime CreatedAt { get; set; }
}

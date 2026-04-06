namespace _66014444_Project.ViewModels.Order;
public class OrderDetailViewModel
{
    // คำสั่งซื้อ
    public int OrderId { get; set; }
    public string OrderNo { get; set; }
    public string OrderStatus { get; set; }
    public string PreviousStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime PaymentDueAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string CancelReason { get; set; }

    // ที่อยู่ snapshot
    public string SnapReceiverName { get; set; }
    public string SnapReceiverPhone { get; set; }
    public string SnapAddressLine1 { get; set; }
    public string SnapAddressLine2 { get; set; }
    public string SnapSubdistrict { get; set; }
    public string SnapDistrict { get; set; }
    public string SnapProvince { get; set; }
    public string SnapPostalCode { get; set; }

    // รายการสินค้า
    public List<OrderItemViewModel> Items { get; set; } = new();

    // ยอดเงิน
    public decimal SubtotalAmount { get; set; }
    public decimal ConditionDiscountAmount { get; set; }
    public decimal PromotionDiscountAmount { get; set; }
    public decimal PointsDiscountAmount { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal TotalAmount { get; set; }
    public int PointsEarned { get; set; }
    public int PointsUsed { get; set; }

    // การชำระเงิน
    public string PaymentStatus { get; set; }
    public decimal? TransferAmount { get; set; }
    public DateTime? TransferDatetime { get; set; }
    public string PayerName { get; set; }
    public string EvidenceUrl { get; set; }
    public string RejectReason { get; set; }
    public bool CanUploadPayment { get; set; }  // true เมื่อ order_status = pending_payment

    // การจัดส่ง
    public string ShipmentStatus { get; set; }
    public string CarrierName { get; set; }
    public string TrackingNo { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
}

public class OrderItemViewModel
{
    public int OrderItemId { get; set; }
    public string BookTitleSnapshot { get; set; }
    public string SeriesNameSnapshot { get; set; }
    public string ConditionCodeSnap { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal NetAmount { get; set; }
}
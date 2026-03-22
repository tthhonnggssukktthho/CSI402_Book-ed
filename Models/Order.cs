using System;
using System.Collections.Generic;

namespace _66014444_Project.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string OrderNo { get; set; } = null!;

    public int CustomerId { get; set; }

    public string SnapReceiverName { get; set; } = null!;

    public string SnapReceiverPhone { get; set; } = null!;

    public string SnapAddressLine1 { get; set; } = null!;

    public string? SnapAddressLine2 { get; set; }

    public string SnapSubdistrict { get; set; } = null!;

    public string SnapDistrict { get; set; } = null!;

    public string SnapProvince { get; set; } = null!;

    public string SnapPostalCode { get; set; } = null!;

    public string OrderStatus { get; set; } = null!;

    public string? PreviousStatus { get; set; }

    public decimal SubtotalAmount { get; set; }

    public decimal ConditionDiscountAmount { get; set; }

    public decimal PromotionDiscountAmount { get; set; }

    public decimal PointsDiscountAmount { get; set; }

    public decimal ShippingFee { get; set; }

    public decimal TotalAmount { get; set; }

    public int PointsEarned { get; set; }

    public int PointsUsed { get; set; }

    public int? PromotionId { get; set; }

    public DateTime PaymentDueAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public string? CancelReason { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<PointTransaction> PointTransactions { get; set; } = new List<PointTransaction>();

    public virtual Promotion? Promotion { get; set; }

    public virtual Shipment? Shipment { get; set; }
}

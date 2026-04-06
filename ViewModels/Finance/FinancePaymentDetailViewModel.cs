using System.ComponentModel.DataAnnotations;
using _66014444_Project.ViewModels.Order;

namespace _66014444_Project.ViewModels.Finance;

public class FinancePaymentDetailViewModel
{
    public int OrderId { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string CustomerDisplayName { get; set; } = string.Empty;
    public string SnapReceiverName { get; set; } = string.Empty;
    public string SnapReceiverPhone { get; set; } = string.Empty;
    public string SnapAddressLine1 { get; set; } = string.Empty;
    public string? SnapAddressLine2 { get; set; }
    public string SnapSubdistrict { get; set; } = string.Empty;
    public string SnapDistrict { get; set; } = string.Empty;
    public string SnapProvince { get; set; } = string.Empty;
    public string SnapPostalCode { get; set; } = string.Empty;
    public List<OrderItemViewModel> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public decimal SubtotalAmount { get; set; }
    public decimal ShippingFee { get; set; }

    public int PaymentId { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public decimal TransferAmount { get; set; }
    public DateTime? TransferDatetime { get; set; }
    public string? PayerName { get; set; }
    public string? EvidenceUrl { get; set; }
    public DateTime? EvidenceUploadedAt { get; set; }

    public string OrderStatus { get; set; } = string.Empty;
    public string ShipmentStatus { get; set; } = string.Empty;
    public string? VerifiedByName { get; set; }
    public DateTime? VerifiedAt { get; set; }

    [StringLength(255)]
    public string? RejectReason { get; set; }

    public bool HasEvidence => !string.IsNullOrWhiteSpace(EvidenceUrl);
    public bool CanTakeAction => PaymentStatus is "submitted" or "pending";
}

using System;
using System.Collections.Generic;

namespace _66014444_Project.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public decimal TransferAmount { get; set; }

    public DateTime? TransferDatetime { get; set; }

    public string? PayerName { get; set; }

    public string? EvidenceUrl { get; set; }

    public DateTime? EvidenceUploadedAt { get; set; }

    public int? VerifiedByEmployeeId { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public string? RejectReason { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Employee? VerifiedByEmployee { get; set; }
}

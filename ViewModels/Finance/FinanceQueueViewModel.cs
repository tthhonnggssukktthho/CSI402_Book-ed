namespace _66014444_Project.ViewModels.Finance;

public class FinanceQueueViewModel
{
    public List<FinanceQueueItemViewModel> Payments { get; set; } = new();
    public string FilterStatus { get; set; } = "submitted";
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class FinanceQueueItemViewModel
{
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string CustomerDisplayName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal TransferAmount { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public string OrderStatus { get; set; } = string.Empty;
    public DateTime? EvidenceUploadedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool HasEvidence { get; set; }
}

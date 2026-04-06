namespace _66014444_Project.ViewModels.Appraisal;

public class AppraisalQueueViewModel
{
    public List<AppraisalQueueItemViewModel> Books { get; set; } = new();
    public string FilterStatus { get; set; } = "pending";
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class AppraisalQueueItemViewModel
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? SeriesName { get; set; }
    public string? VolumeNo { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? PublisherName { get; set; }
    public string? AuthorName { get; set; }
    public string SellerDisplayName { get; set; } = string.Empty;
    public string SellerUsername { get; set; } = string.Empty;
    public string ConditionCode { get; set; } = string.Empty;
    public string? ConditionNote { get; set; }
    public decimal? ProposedPrice { get; set; }
    public decimal? ApprovedPrice { get; set; }
    public string ApprovalStatus { get; set; } = string.Empty;
    public string SaleStatus { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

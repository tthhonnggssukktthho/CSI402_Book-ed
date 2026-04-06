namespace _66014444_Project.ViewModels.Appraisal;

public class AppraisalReviewViewModel
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? SeriesName { get; set; }
    public string? VolumeNo { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? PublisherName { get; set; }
    public string? AuthorName { get; set; }
    public string? Isbn { get; set; }
    public int? PublishYear { get; set; }
    public string? Synopsis { get; set; }
    public string? BookDescription { get; set; }
    public string ConditionCode { get; set; } = string.Empty;
    public string? ConditionNote { get; set; }
    public decimal? ProposedPrice { get; set; }
    public decimal? ApprovedPrice { get; set; }
    public string ApprovalStatus { get; set; } = string.Empty;
    public string SaleStatus { get; set; } = string.Empty;
    public string? RejectionReason { get; set; }
    public string SellerDisplayName { get; set; } = string.Empty;
    public string SellerUsername { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? ImageUrl2 { get; set; }
    public string? ImageUrl3 { get; set; }
    public string? ImageUrl4 { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

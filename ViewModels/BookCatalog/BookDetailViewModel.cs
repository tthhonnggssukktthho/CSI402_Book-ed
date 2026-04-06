namespace _66014444_Project.ViewModels.BookCatalog;
public class BookDetailViewModel
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
    public string? ImageUrl { get; set; }
    public string? ImageUrl2 { get; set; }
    public string? ImageUrl3 { get; set; }
    public string? ImageUrl4 { get; set; }
    public string ConditionCode { get; set; } = string.Empty;
    public decimal ConditionDiscountPct { get; set; }
    public string? ConditionNote { get; set; }
    public decimal ApprovedPrice { get; set; }
    public decimal FinalPrice { get; set; }
    public string SaleStatus { get; set; } = string.Empty;
    public bool CanAddToCart { get; set; }
}
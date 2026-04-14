namespace _66014444_Project.ViewModels.Customer;

public class MyBooksViewModel
{
    public List<PurchasedBookItemViewModel> PurchasedBooks { get; set; } = new();
    public List<MyBookItemViewModel> SellingBooks { get; set; } = new();
}

public class PurchasedBookItemViewModel
{
    public int OrderId { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? SeriesName { get; set; }
    public string ConditionCode { get; set; } = string.Empty;
    public decimal NetAmount { get; set; }
    public DateTime PurchasedAt { get; set; }
    public string OrderStatus { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}

public class MyBookItemViewModel
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? SeriesName { get; set; }
    public string? VolumeNo { get; set; }
    public string ConditionCode { get; set; } = string.Empty;
    public string? ConditionNote { get; set; }
    public decimal? ProposedPrice { get; set; }
    public decimal? ApprovedPrice { get; set; }
    public string ApprovalStatus { get; set; } = string.Empty;
    public string SaleStatus { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool CanEdit { get; set; }
}

namespace _66014444_Project.ViewModels.BookCatalog;

public class BookCatalogIndexViewModel
{
    public List<BookCardViewModel> Books { get; set; } = new();
    public string? SearchKeyword { get; set; }
    public string? FilterCategory { get; set; }
    public string? FilterCondition { get; set; }
    public decimal? PriceMin { get; set; }
    public decimal? PriceMax { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int TotalItems { get; set; }
    public int PageSize { get; set; } = 12;
    public List<string> Categories { get; set; } = new();
}

public class BookCardViewModel
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? SeriesName { get; set; }
    public string? VolumeNo { get; set; }
    public string? AuthorName { get; set; }
    public string? CategoryName { get; set; }
    public string? ImageUrl { get; set; }
    public string ConditionCode { get; set; } = string.Empty;
    public decimal ConditionDiscountPct { get; set; }
    public decimal ApprovedPrice { get; set; }
    public decimal FinalPrice { get; set; }
    public string? ConditionNote { get; set; }
}

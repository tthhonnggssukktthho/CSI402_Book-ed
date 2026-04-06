namespace _66014444_Project.ViewModels.Cart;

public class CartViewModel
{
    public List<CartItemViewModel> Items { get; set; } = new();
    public decimal Subtotal { get; set; }
    public bool IsEmpty => !Items.Any();
}

public class CartItemViewModel
{
    public int CartItemId { get; set; }
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? SeriesName { get; set; }
    public string? VolumeNo { get; set; }
    public string? ImageUrl { get; set; }
    public string ConditionCode { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal ConditionDiscountPct { get; set; }
    public decimal FinalPrice { get; set; }
    public string SaleStatus { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public DateTime AddedAt { get; set; }
}

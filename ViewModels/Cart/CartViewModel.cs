

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
    public string Title { get; set; }
    public string SeriesName { get; set; }
    public string VolumeNo { get; set; }
    public string ImageUrl { get; set; }
    public string ConditionCode { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ConditionDiscountPct { get; set; }
    public decimal FinalPrice { get; set; }
    public string SaleStatus { get; set; }        // เช็กว่ายังพร้อมขายอยู่ไหม
    public bool IsAvailable { get; set; }         // false = หนังสือถูกซื้อไปแล้ว
}
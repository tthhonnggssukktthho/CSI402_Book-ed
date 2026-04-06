namespace _66014444_Project.ViewModels.Order;

public class OrderIndexViewModel
{
    public List<OrderSummaryViewModel> Orders { get; set; } = new();

    // filter
    public string FilterStatus { get; set; }  // all | pending_payment | payment_submitted | ... | completed

    // pagination
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 10;
}

public class OrderSummaryViewModel
{
    public int OrderId { get; set; }
    public string OrderNo { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ItemCount { get; set; }
    public decimal TotalAmount { get; set; }
    public string OrderStatus { get; set; }
    public DateTime PaymentDueAt { get; set; }
    public bool IsExpiringSoon { get; set; }  // true เมื่อ pending_payment และเหลือ < 1 ชม.
}
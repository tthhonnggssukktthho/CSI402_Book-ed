using System;
using System.Collections.Generic;

namespace _66014444_Project.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int BookId { get; set; }

    public int SellerCustomerId { get; set; }

    public string BookTitleSnapshot { get; set; } = null!;

    public string? SeriesNameSnapshot { get; set; }

    public string ConditionCodeSnap { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal NetAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Customer SellerCustomer { get; set; } = null!;
}

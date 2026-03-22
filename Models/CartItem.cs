using System;
using System.Collections.Generic;

namespace _66014444_Project.Models;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int CustomerId { get; set; }

    public int BookId { get; set; }

    public decimal UnitPrice { get; set; }

    public DateTime AddedAt { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace _66014444_Project.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int UserId { get; set; }

    public string DisplayName { get; set; } = null!;

    public DateOnly? BirthDate { get; set; }

    public string Status { get; set; } = null!;

    public string? ReceiverName { get; set; }

    public string? ReceiverPhone { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? Subdistrict { get; set; }

    public string? District { get; set; }

    public string? Province { get; set; }

    public string? PostalCode { get; set; }

    public int CurrentPoints { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<PointTransaction> PointTransactions { get; set; } = new List<PointTransaction>();

    public virtual User User { get; set; } = null!;
}

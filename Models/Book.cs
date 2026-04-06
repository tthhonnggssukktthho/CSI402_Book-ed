using System;
using System.Collections.Generic;

namespace _66014444_Project.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public string? SeriesName { get; set; }

    public string? VolumeNo { get; set; }

    public string CategoryName { get; set; } = null!;

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

    public string ConditionCode { get; set; } = null!;

    public decimal ConditionDiscountPct { get; set; }

    public string? ConditionNote { get; set; }

    public decimal? ProposedPrice { get; set; }

    public decimal? ApprovedPrice { get; set; }

    public string ApprovalStatus { get; set; } = null!;

    public string SaleStatus { get; set; } = null!;

    public int? ReviewedByEmployeeId { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public string? RejectionReason { get; set; }

    public int SellerCustomerId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<PromotionRule> PromotionRules { get; set; } = new List<PromotionRule>();

    public virtual Employee? ReviewedByEmployee { get; set; }

    public virtual Customer SellerCustomer { get; set; } = null!;
}

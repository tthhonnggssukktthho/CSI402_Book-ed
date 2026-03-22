using System;
using System.Collections.Generic;

namespace _66014444_Project.Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string PromotionCode { get; set; } = null!;

    public string PromotionName { get; set; } = null!;

    public string PromotionType { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? DiscountType { get; set; }

    public decimal? DiscountValue { get; set; }

    public decimal? MinOrderAmount { get; set; }

    public int? MinItemQty { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime? EndAt { get; set; }

    public bool? IsActive { get; set; }

    public bool IsAutoApply { get; set; }

    public int? CreatedByEmployeeId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Employee? CreatedByEmployee { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<PromotionRule> PromotionRules { get; set; } = new List<PromotionRule>();
}

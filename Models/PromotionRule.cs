using System;
using System.Collections.Generic;

namespace _66014444_Project.Models;

public partial class PromotionRule
{
    public int RuleId { get; set; }

    public int PromotionId { get; set; }

    public string RuleType { get; set; } = null!;

    public string? RuleOperator { get; set; }

    public string? RuleValue { get; set; }

    public int? BookId { get; set; }

    public string? SeriesName { get; set; }

    public string? CategoryName { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Promotion Promotion { get; set; } = null!;
}

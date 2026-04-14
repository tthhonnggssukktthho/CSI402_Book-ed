namespace _66014444_Project.ViewModels.Checkout;

public class CheckoutViewModel
{
    public string? ReceiverName { get; set; }
    public string? ReceiverPhone { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? Subdistrict { get; set; }
    public string? District { get; set; }
    public string? Province { get; set; }
    public string? PostalCode { get; set; }

    public List<CheckoutItemViewModel> Items { get; set; } = new();
    public List<CheckoutAutomaticPromotionViewModel> AutomaticPromotions { get; set; } = new();
    public List<CheckoutPromotionViewModel> Promotions { get; set; } = new();

    public int? SelectedPromotionId { get; set; }
    public string? AppliedPromotionName { get; set; }
    public string? PromotionErrorMessage { get; set; }

    public int AvailablePoints { get; set; }
    public int MaxPointsToUse { get; set; }
    public int PointsToUse { get; set; }
    public decimal PointsDiscountAmount { get; set; }

    public decimal SubtotalAmount { get; set; }
    public decimal ConditionDiscountAmount { get; set; }
    public decimal PromotionDiscountAmount { get; set; }
    public decimal AutoPromotionDiscountAmount { get; set; }
    public decimal SelectedPromotionDiscountAmount { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal TotalAmount { get; set; }
    public int PointsToEarn { get; set; }

    public bool HasShippingAddress =>
        !string.IsNullOrWhiteSpace(ReceiverName) &&
        !string.IsNullOrWhiteSpace(ReceiverPhone) &&
        !string.IsNullOrWhiteSpace(AddressLine1) &&
        !string.IsNullOrWhiteSpace(Subdistrict) &&
        !string.IsNullOrWhiteSpace(District) &&
        !string.IsNullOrWhiteSpace(Province) &&
        !string.IsNullOrWhiteSpace(PostalCode);
}

public class CheckoutItemViewModel
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? SeriesName { get; set; }
    public string ConditionCode { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal ConditionDiscountPct { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal NetAmount { get; set; }
    public string? ImageUrl { get; set; }
}

public class CheckoutAutomaticPromotionViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal DiscountAmount { get; set; }
    public bool IsApplied { get; set; }
}

public class CheckoutPromotionViewModel
{
    public int PromotionId { get; set; }
    public string PromotionCode { get; set; } = string.Empty;
    public string PromotionName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PromotionType { get; set; } = string.Empty;
    public string? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    public decimal? MinOrderAmount { get; set; }
    public int? MinItemQty { get; set; }
    public string? TargetSeriesName { get; set; }
    public string RuleLabel { get; set; } = string.Empty;
    public bool IsEligible { get; set; }
    public decimal CalculatedDiscountAmount { get; set; }
    public bool IsSelected { get; set; }
}

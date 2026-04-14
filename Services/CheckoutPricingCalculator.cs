using Microsoft.EntityFrameworkCore;
using _66014444_Project.Models;

namespace _66014444_Project.Services;

public static class CheckoutPricingCalculator
{
    public static CheckoutPricingResult Calculate(
        IReadOnlyCollection<CartItem> cartItems,
        IReadOnlyCollection<Promotion> promotions,
        int availablePoints = 0,
        int? selectedPromotionId = null,
        int requestedPointsToUse = 0)
    {
        var lines = cartItems
            .Select(item =>
            {
                var conditionDiscountPct = ConditionDiscountHelper.ResolvePercent(item.Book.ConditionCode, item.Book.ConditionDiscountPct);
                var conditionDiscountAmount = item.UnitPrice * (conditionDiscountPct / 100m);
                return new CheckoutPricingLine
                {
                    CartItem = item,
                    NetAmountAfterCondition = item.UnitPrice - conditionDiscountAmount,
                    ConditionDiscountAmount = conditionDiscountAmount
                };
            })
            .ToList();

        var subtotalAmount = lines.Sum(line => line.CartItem.UnitPrice);
        var conditionDiscountAmount = lines.Sum(line => line.ConditionDiscountAmount);
        var subtotalAfterCondition = lines.Sum(line => line.NetAmountAfterCondition);

        var groupedSeries = lines
            .Where(line => !string.IsNullOrWhiteSpace(line.CartItem.Book.SeriesName))
            .GroupBy(line => line.CartItem.Book.SeriesName!.Trim(), StringComparer.OrdinalIgnoreCase)
            .ToList();

        var eligibleSeriesDiscountCount = groupedSeries.Count(group => group.Count() >= 2);
        var sameSeriesDiscountAmount = eligibleSeriesDiscountCount * 50m;

        var flashSalePromotions = promotions
            .Where(p => string.Equals(p.PromotionType, "flash_sale", StringComparison.OrdinalIgnoreCase))
            .Select(p => BuildEligiblePromotion(p, lines, subtotalAfterCondition))
            .ToList();

        var selectedPromotion = flashSalePromotions
            .FirstOrDefault(p => p.PromotionId == selectedPromotionId && p.IsEligible);

        var selectedPromotionDiscountAmount = selectedPromotion?.DiscountAmount ?? 0m;
        var freeShippingApplied = subtotalAfterCondition >= 500m;
        var shippingFee = lines.Count == 0 ? 0m : freeShippingApplied ? 0m : 50m;
        var promotionDiscountAmount = sameSeriesDiscountAmount + selectedPromotionDiscountAmount;
        var amountBeforePoints = Math.Max(0m, subtotalAfterCondition - promotionDiscountAmount + shippingFee);
        var maxPointsToUse = Math.Min(availablePoints, (int)Math.Floor(amountBeforePoints));
        var pointsToUse = Math.Clamp(requestedPointsToUse, 0, maxPointsToUse);
        var totalAmount = Math.Max(0m, amountBeforePoints - pointsToUse);

        return new CheckoutPricingResult
        {
            SubtotalAmount = subtotalAmount,
            ConditionDiscountAmount = conditionDiscountAmount,
            SubtotalAfterCondition = subtotalAfterCondition,
            SameSeriesDiscountAmount = sameSeriesDiscountAmount,
            SameSeriesEligibleCount = eligibleSeriesDiscountCount,
            ShippingFee = shippingFee,
            FreeShippingApplied = freeShippingApplied,
            PromotionDiscountAmount = promotionDiscountAmount,
            SelectedPromotionId = selectedPromotion?.PromotionId,
            SelectedPromotionName = selectedPromotion?.PromotionName,
            SelectedPromotionDiscountAmount = selectedPromotionDiscountAmount,
            MaxPointsToUse = maxPointsToUse,
            PointsToUse = pointsToUse,
            PointsDiscountAmount = pointsToUse,
            TotalAmount = totalAmount,
            PointsToEarn = (int)Math.Floor(totalAmount / 100m),
            EligiblePromotions = flashSalePromotions
        };
    }

    private static EligiblePromotionResult BuildEligiblePromotion(
        Promotion promotion,
        List<CheckoutPricingLine> lines,
        decimal subtotalAfterCondition)
    {
        var matchedLines = lines
            .Where(line => MatchesAnyRule(line.CartItem.Book, promotion.PromotionRules))
            .ToList();

        var matchedItemCount = matchedLines.Count;
        var matchedSubtotal = matchedLines.Sum(line => line.NetAmountAfterCondition);
        var minOrderAmount = promotion.MinOrderAmount ?? 0m;
        var minItemQty = promotion.MinItemQty ?? 0;
        var isEligible = matchedItemCount > 0 &&
                         subtotalAfterCondition >= minOrderAmount &&
                         matchedItemCount >= minItemQty;

        var discountAmount = 0m;
        if (isEligible)
        {
            discountAmount = string.Equals(promotion.DiscountType, "percent", StringComparison.OrdinalIgnoreCase)
                ? matchedSubtotal * ((promotion.DiscountValue ?? 0m) / 100m)
                : promotion.DiscountValue ?? 0m;
        }

        return new EligiblePromotionResult
        {
            PromotionId = promotion.PromotionId,
            PromotionCode = promotion.PromotionCode,
            PromotionName = promotion.PromotionName,
            Description = promotion.Description,
            DiscountType = promotion.DiscountType,
            DiscountValue = promotion.DiscountValue,
            MinOrderAmount = promotion.MinOrderAmount,
            MinItemQty = promotion.MinItemQty,
            IsEligible = isEligible,
            DiscountAmount = discountAmount,
            TargetSeriesName = promotion.PromotionRules
                .Where(rule => !string.IsNullOrWhiteSpace(rule.SeriesName))
                .Select(rule => rule.SeriesName!)
                .FirstOrDefault(),
            RuleLabel = BuildRuleLabel(promotion.PromotionRules)
        };
    }

    private static bool MatchesAnyRule(Book book, IEnumerable<PromotionRule> rules)
    {
        var ruleList = rules.ToList();
        if (ruleList.Count == 0)
        {
            return false;
        }

        return ruleList.Any(rule => MatchesRule(book, rule));
    }

    private static bool MatchesRule(Book book, PromotionRule rule)
    {
        return (rule.RuleType ?? string.Empty).ToLowerInvariant() switch
        {
            "series_name" => string.Equals(book.SeriesName?.Trim(), rule.SeriesName?.Trim() ?? rule.RuleValue?.Trim(), StringComparison.OrdinalIgnoreCase),
            "book_id" => rule.BookId.HasValue && book.BookId == rule.BookId.Value,
            "category_name" => string.Equals(book.CategoryName?.Trim(), rule.CategoryName?.Trim() ?? rule.RuleValue?.Trim(), StringComparison.OrdinalIgnoreCase),
            _ => false
        };
    }

    private static string BuildRuleLabel(IEnumerable<PromotionRule> rules)
    {
        var rule = rules.FirstOrDefault();
        if (rule is null)
        {
            return "-";
        }

        return (rule.RuleType ?? string.Empty).ToLowerInvariant() switch
        {
            "series_name" => $"ชุดนิยาย: {rule.SeriesName ?? rule.RuleValue ?? "-"}",
            "book_id" => $"หนังสือรหัส: {rule.BookId?.ToString() ?? "-"}",
            "category_name" => $"หมวดหมู่: {rule.CategoryName ?? rule.RuleValue ?? "-"}",
            _ => rule.RuleValue ?? "-"
        };
    }
}

public class CheckoutPricingResult
{
    public decimal SubtotalAmount { get; set; }
    public decimal ConditionDiscountAmount { get; set; }
    public decimal SubtotalAfterCondition { get; set; }
    public decimal SameSeriesDiscountAmount { get; set; }
    public int SameSeriesEligibleCount { get; set; }
    public bool FreeShippingApplied { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal PromotionDiscountAmount { get; set; }
    public int? SelectedPromotionId { get; set; }
    public string? SelectedPromotionName { get; set; }
    public decimal SelectedPromotionDiscountAmount { get; set; }
    public int MaxPointsToUse { get; set; }
    public int PointsToUse { get; set; }
    public decimal PointsDiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public int PointsToEarn { get; set; }
    public List<EligiblePromotionResult> EligiblePromotions { get; set; } = new();
}

public class EligiblePromotionResult
{
    public int PromotionId { get; set; }
    public string PromotionCode { get; set; } = string.Empty;
    public string PromotionName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    public decimal? MinOrderAmount { get; set; }
    public int? MinItemQty { get; set; }
    public bool IsEligible { get; set; }
    public decimal DiscountAmount { get; set; }
    public string? TargetSeriesName { get; set; }
    public string RuleLabel { get; set; } = string.Empty;
}

internal class CheckoutPricingLine
{
    public CartItem CartItem { get; set; } = null!;
    public decimal NetAmountAfterCondition { get; set; }
    public decimal ConditionDiscountAmount { get; set; }
}

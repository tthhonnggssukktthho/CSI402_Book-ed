namespace _66014444_Project.Services;

public static class ConditionDiscountHelper
{
    public static decimal ResolvePercent(string? conditionCode, decimal storedPercent = 0m)
    {
        if (storedPercent > 0)
        {
            return storedPercent;
        }

        return (conditionCode ?? string.Empty).Trim().ToUpperInvariant() switch
        {
            "LIKE_NEW" => 5m,
            "GOOD" => 10m,
            "MINOR_DEFECT" => 15m,
            "MAJOR_DEFECT" => 20m,
            _ => 0m
        };
    }
}

using System.ComponentModel.DataAnnotations;

namespace _66014444_Project.ViewModels.Finance;

public class PromotionIndexViewModel
{
    public List<SystemPromotionCardViewModel> SystemPromotions { get; set; } = new();
    public List<PromotionSummaryViewModel> Promotions { get; set; } = new();
    public string FilterType { get; set; } = string.Empty;
    public bool? FilterActive { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 20;
}

public class SystemPromotionCardViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string AutoApplyNote { get; set; } = string.Empty;
}

public class PromotionSummaryViewModel
{
    public int PromotionId { get; set; }
    public string PromotionCode { get; set; } = string.Empty;
    public string PromotionName { get; set; } = string.Empty;
    public string PromotionType { get; set; } = string.Empty;
    public string DiscountType { get; set; } = string.Empty;
    public decimal? DiscountValue { get; set; }
    public decimal? MinOrderAmount { get; set; }
    public int? MinItemQty { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsAutoApply { get; set; }
    public bool IsExpired { get; set; }
    public string CreatedByEmployeeName { get; set; } = string.Empty;
    public string RuleType { get; set; } = string.Empty;
    public string RuleOperator { get; set; } = string.Empty;
    public string RuleValue { get; set; } = string.Empty;
    public string TargetSeriesName { get; set; } = string.Empty;
    public int? TargetBookId { get; set; }
    public string? TargetCategoryName { get; set; }
    public int RuleCount { get; set; }
}

public class PromotionFormViewModel
{
    public int PromotionId { get; set; }

    [Required(ErrorMessage = "กรุณากรอกรหัสโปรโมชัน")]
    [StringLength(50)]
    public string PromotionCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "กรุณากรอกชื่อโปรโมชัน")]
    [StringLength(150)]
    public string PromotionName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "กรุณาเลือกประเภทส่วนลด")]
    public string DiscountType { get; set; } = "percent";

    [Range(0.01, 999999, ErrorMessage = "กรุณากรอกมูลค่าส่วนลดให้ถูกต้อง")]
    public decimal DiscountValue { get; set; }

    [Range(0, 999999, ErrorMessage = "กรุณากรอกราคาขั้นต่ำให้ถูกต้อง")]
    public decimal? MinOrderAmount { get; set; }

    [Range(0, 9999, ErrorMessage = "กรุณากรอกจำนวนหนังสือขั้นต่ำให้ถูกต้อง")]
    public int? MinItemQty { get; set; }

    [Required(ErrorMessage = "กรุณาเลือกวันเริ่มต้น")]
    public DateTime StartAt { get; set; }

    [Required(ErrorMessage = "กรุณาเลือกวันสิ้นสุด")]
    public DateTime EndAt { get; set; }

    [Required(ErrorMessage = "กรุณาเลือกประเภทเงื่อนไข")]
    public string RuleType { get; set; } = "series_name";

    [Required(ErrorMessage = "กรุณาเลือกตัวดำเนินการเงื่อนไข")]
    public string RuleOperator { get; set; } = "equals";

    public string RuleValue { get; set; } = string.Empty;

    public string SelectedSeriesName { get; set; } = string.Empty;
    public int? TargetBookId { get; set; }
    public string? TargetCategoryName { get; set; }
    public int? CreatedByEmployeeId { get; set; }
    public string CreatedByEmployeeName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public List<string> AvailableSeriesNames { get; set; } = new();
    public List<string> AvailableCategoryNames { get; set; } = new();
    public List<PromotionSeriesBookViewModel> AvailableBooks { get; set; } = new();
}

public class PromotionSeriesBookViewModel
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string SeriesName { get; set; } = string.Empty;
    public string? VolumeNo { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal? ApprovedPrice { get; set; }
    public string? ImageUrl { get; set; }
}

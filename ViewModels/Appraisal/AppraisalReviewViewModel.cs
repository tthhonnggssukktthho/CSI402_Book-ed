using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _66014444_Project.ViewModels.Appraisal;

public class AppraisalReviewViewModel
{
    public int BookId { get; set; }

    [Required]
    [StringLength(255)]
    [Display(Name = "ชื่อหนังสือ")]
    public string Title { get; set; } = string.Empty;

    [StringLength(255)]
    [Display(Name = "ชื่อซีรีส์")]
    public string? SeriesName { get; set; }

    [StringLength(20)]
    [Display(Name = "เล่มที่")]
    public string? VolumeNo { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "หมวดหมู่")]
    public string CategoryName { get; set; } = string.Empty;

    [StringLength(150)]
    [Display(Name = "สำนักพิมพ์")]
    public string? PublisherName { get; set; }

    [StringLength(150)]
    [Display(Name = "ผู้แต่ง")]
    public string? AuthorName { get; set; }

    [StringLength(20)]
    [Display(Name = "ISBN")]
    public string? Isbn { get; set; }

    [Range(1000, 9999)]
    [Display(Name = "ปีที่พิมพ์")]
    public int? PublishYear { get; set; }

    [Display(Name = "เรื่องย่อ")]
    public string? Synopsis { get; set; }

    [Display(Name = "รายละเอียดหนังสือ")]
    public string? BookDescription { get; set; }

    [Required]
    [Display(Name = "ระดับสภาพหนังสือ")]
    public string ConditionCode { get; set; } = string.Empty;

    [Display(Name = "หมายเหตุสภาพหนังสือ")]
    public string? ConditionNote { get; set; }

    [Display(Name = "ราคาที่ผู้ขายเสนอ")]
    public decimal? ProposedPrice { get; set; }

    [Display(Name = "ราคาที่อนุมัติ")]
    [Range(0.01, 999999)]
    public decimal? ApprovedPrice { get; set; }

    public string ApprovalStatus { get; set; } = string.Empty;
    public string SaleStatus { get; set; } = string.Empty;

    [Display(Name = "เหตุผลที่ไม่อนุมัติ")]
    public string? RejectionReason { get; set; }

    public string SellerDisplayName { get; set; } = string.Empty;
    public string SellerUsername { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? ImageUrl2 { get; set; }
    public string? ImageUrl3 { get; set; }
    public string? ImageUrl4 { get; set; }
    public string? ReviewedByName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }

    public List<string> Categories { get; set; } = new();
    public List<SelectListItem> ConditionOptions { get; set; } = new();
}

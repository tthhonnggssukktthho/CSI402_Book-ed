using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _66014444_Project.ViewModels.SellBook;

public class SellBookEditViewModel : IValidatableObject
{
    public int BookId { get; set; }

    [Required(ErrorMessage = "กรุณากรอกชื่อหนังสือ")]
    [StringLength(255)]
    [Display(Name = "ชื่อหนังสือ")]
    public string Title { get; set; } = string.Empty;

    [StringLength(255)]
    [Display(Name = "ชื่อซีรีส์")]
    public string? SeriesName { get; set; }

    [StringLength(20)]
    [Display(Name = "เล่มที่")]
    public string? VolumeNo { get; set; }

    [Required(ErrorMessage = "กรุณาเลือกหมวดหมู่หนังสือ")]
    [StringLength(100)]
    [Display(Name = "หมวดหมู่หนังสือ")]
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

    [Range(1000, 9999, ErrorMessage = "กรุณาใส่ปี พ.ศ. ที่ถูกต้อง")]
    [Display(Name = "ปีที่พิมพ์ ปี พ.ศ.")]
    public int? PublishYear { get; set; }

    [Display(Name = "เรื่องย่อ")]
    public string? Synopsis { get; set; }

    [Display(Name = "รายละเอียดหนังสือ")]
    public string? BookDescription { get; set; }

    [Display(Name = "รูปปกหน้า")]
    public IFormFile? ImageFile1 { get; set; }

    [Display(Name = "รูปปกหลัง")]
    public IFormFile? ImageFile2 { get; set; }

    [Display(Name = "รูปตำหนิ 1")]
    public IFormFile? ImageFile3 { get; set; }

    [Display(Name = "รูปตำหนิ 2")]
    public IFormFile? ImageFile4 { get; set; }

    public string? ExistingImageUrl1 { get; set; }
    public string? ExistingImageUrl2 { get; set; }
    public string? ExistingImageUrl3 { get; set; }
    public string? ExistingImageUrl4 { get; set; }

    [Required(ErrorMessage = "กรุณากรอกรายละเอียดสภาพหนังสือ")]
    [StringLength(255)]
    [Display(Name = "หมายเหตุสภาพหนังสือ")]
    public string ConditionNote { get; set; } = string.Empty;

    [Required(ErrorMessage = "กรุณาใส่ราคาที่ต้องการขาย")]
    [Range(0.01, 99999)]
    [Display(Name = "ราคาที่ต้องการขาย")]
    public decimal? ProposedPrice { get; set; }

    [Required(ErrorMessage = "กรุณาเลือกสภาพหนังสือ")]
    [Display(Name = "ระดับสภาพหนังสือ")]
    public string ConditionCode { get; set; } = string.Empty;

    public string ApprovalStatus { get; set; } = string.Empty;
    public string SaleStatus { get; set; } = string.Empty;
    public string? RejectionReason { get; set; }

    public List<string> Categories { get; set; } = new();
    public List<SelectListItem> ConditionOptions { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(ExistingImageUrl1) && ImageFile1 is null)
        {
            yield return new ValidationResult("กรุณาอัปโหลดรูปปกหน้า", [nameof(ImageFile1)]);
        }

        if (string.IsNullOrWhiteSpace(ExistingImageUrl2) && ImageFile2 is null)
        {
            yield return new ValidationResult("กรุณาอัปโหลดรูปปกหลัง", [nameof(ImageFile2)]);
        }

        if (string.IsNullOrWhiteSpace(ExistingImageUrl3) && ImageFile3 is null)
        {
            yield return new ValidationResult("กรุณาอัปโหลดรูปตำหนิ 1", [nameof(ImageFile3)]);
        }

        if (string.IsNullOrWhiteSpace(ExistingImageUrl4) && ImageFile4 is null)
        {
            yield return new ValidationResult("กรุณาอัปโหลดรูปตำหนิ 2", [nameof(ImageFile4)]);
        }
    }
}

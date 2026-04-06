using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _66014444_Project.ViewModels.SellBook;

public class SellBookCreateViewModel
{
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

    [Range(1000, 9999, ErrorMessage = "กรุณาใส่ปีพ.ศ.ที่ถูกต้อง")]
    [Display(Name = "ปีที่พิมพ์ ปี พ.ศ.")]
    public int? PublishYear { get; set; }

    [Display(Name = "เรื่องย่อ")]
    public string? Synopsis { get; set; }

    [Display(Name = "รายละเอียดหนังสือ")]
    public string? BookDescription { get; set; }

    [Required(ErrorMessage = "กรุณาอัปโหลดรูปปกหน้า")]
    [Display(Name = "รูปปกหน้า")]
    public IFormFile? ImageFile1 { get; set; }

    [Required(ErrorMessage = "กรุณาอัปโหลดรูปปกหลัง")]
    [Display(Name = "รูปปกหลัง")]
    public IFormFile? ImageFile2 { get; set; }

    [Required(ErrorMessage = "กรุณาอัปโหลดรูปตำหนิ1")]
    [Display(Name = "รูปตำหนิ1")]
    public IFormFile? ImageFile3 { get; set; }

    [Required(ErrorMessage = "กรุณาอัปโหลดรูปตำหนิ2")]
    [Display(Name = "รูปตำหนิ2")]
    public IFormFile? ImageFile4 { get; set; }

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

    public List<string> Categories { get; set; } = new();
    public List<SelectListItem> ConditionOptions { get; set; } = new();
}

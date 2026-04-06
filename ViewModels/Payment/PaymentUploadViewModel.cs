using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace _66014444_Project.ViewModels.Payment;

public class PaymentUploadViewModel
{
    public int OrderId { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime PaymentDueAt { get; set; }

    [Required(ErrorMessage = "กรุณากรอกจำนวนเงินที่โอน")]
    [Range(0.01, double.MaxValue)]
    public decimal TransferAmount { get; set; }

    [Required(ErrorMessage = "กรุณากรอกวันเวลาที่โอน")]
    public DateTime? TransferDatetime { get; set; }

    [StringLength(100)]
    public string? PayerName { get; set; }

    [Required(ErrorMessage = "กรุณาแนบหลักฐานการชำระเงิน")]
    public IFormFile? EvidenceFile { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace _66014444_Project.ViewModels.Customer;

public class ProfileViewModel
{
    public int CustomerId { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Display Name")]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [StringLength(20)]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Birth Date")]
    public DateOnly? BirthDate { get; set; }

    [Display(Name = "Status")]
    public string Status { get; set; } = string.Empty;

    [Display(Name = "Receiver Name")]
    [StringLength(100)]
    public string? ReceiverName { get; set; }

    [Display(Name = "Receiver Phone")]
    [Phone]
    [StringLength(20)]
    public string? ReceiverPhone { get; set; }

    [Display(Name = "Address Line 1")]
    [StringLength(255)]
    public string? AddressLine1 { get; set; }

    [Display(Name = "Address Line 2")]
    [StringLength(255)]
    public string? AddressLine2 { get; set; }

    [Display(Name = "Subdistrict")]
    [StringLength(100)]
    public string? Subdistrict { get; set; }

    [Display(Name = "District")]
    [StringLength(100)]
    public string? District { get; set; }

    [Display(Name = "Province")]
    [StringLength(100)]
    public string? Province { get; set; }

    [Display(Name = "Postal Code")]
    [StringLength(10)]
    public string? PostalCode { get; set; }

    public int CurrentPoints { get; set; }
    public DateTime CreatedAt { get; set; }
}

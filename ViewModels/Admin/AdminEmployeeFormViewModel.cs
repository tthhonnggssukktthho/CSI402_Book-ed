using System.ComponentModel.DataAnnotations;

namespace _66014444_Project.ViewModels.Admin;

public class AdminEmployeeFormViewModel
{
    public static readonly string[] EmployeeRoles = ["admin", "appraisal", "finance", "shipping"];

    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(255, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare("Password", ErrorMessage = "Confirm password does not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [Display(Name = "Role")]
    public string Role { get; set; } = string.Empty;

    [Phone]
    [StringLength(20)]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Hire Date")]
    public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    public string? GeneratedEmployeeCode { get; set; }
}

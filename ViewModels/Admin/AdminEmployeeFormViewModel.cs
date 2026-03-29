using System.ComponentModel.DataAnnotations;

namespace _66014444_Project.ViewModels.Admin;

public class AdminEmployeeFormViewModel : IValidatableObject
{
    public static readonly string[] EmployeeRoles = ["admin", "appraisal", "finance", "shipping"];
    public static readonly string[] EmploymentStatuses = ["active", "inactive", "resigned"];

    public int? EmployeeId { get; set; }

    public bool IsEditMode => EmployeeId.HasValue;

    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [StringLength(255, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

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

    [Display(Name = "Employee Code")]
    public string? EmployeeCode { get; set; }

    [Phone]
    [StringLength(20)]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [StringLength(255)]
    [Display(Name = "Address Line 1")]
    public string? Address1 { get; set; }

    [StringLength(255)]
    [Display(Name = "Address Line 2")]
    public string? Address2 { get; set; }

    [StringLength(100)]
    [Display(Name = "Subdistrict")]
    public string? Subdistrict { get; set; }

    [StringLength(100)]
    [Display(Name = "District")]
    public string? District { get; set; }

    [StringLength(100)]
    [Display(Name = "Province")]
    public string? Province { get; set; }

    [StringLength(10)]
    [Display(Name = "Postal Code")]
    public string? PostalCode { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Hire Date")]
    public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Employment Status")]
    public string EmploymentStatus { get; set; } = "active";

    [DataType(DataType.Date)]
    [Display(Name = "Resign Date")]
    public DateOnly? ResignDate { get; set; }

    public string? GeneratedEmployeeCode { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            yield return new ValidationResult("Username is required.", [nameof(Username)]);
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            yield return new ValidationResult("Email is required.", [nameof(Email)]);
        }

        if (!IsEditMode && string.IsNullOrWhiteSpace(Password))
        {
            yield return new ValidationResult("Password is required.", [nameof(Password)]);
        }

        if (!IsEditMode && string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            yield return new ValidationResult("Confirm password is required.", [nameof(ConfirmPassword)]);
        }

        if (!string.IsNullOrWhiteSpace(Password) && Password.Length < 6)
        {
            yield return new ValidationResult("Password must be at least 6 characters.", [nameof(Password)]);
        }

        if (IsEditMode && !string.IsNullOrWhiteSpace(Password) && string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            yield return new ValidationResult("Please confirm the new password.", [nameof(ConfirmPassword)]);
        }
    }
}

using System.ComponentModel.DataAnnotations;
namespace _66014444_Project.ViewModels
{
    public class RegisterViewModel
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "รหัสผ่านไม่ตรงกัน")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    [Required]
    [StringLength(100)]
    public string DisplayName { get; set; }

    [Phone]
    [StringLength(20)]
    public string PhoneNumber { get; set; }
}
}


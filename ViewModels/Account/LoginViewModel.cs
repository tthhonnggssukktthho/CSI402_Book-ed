using System.ComponentModel.DataAnnotations;
namespace _66014444_Project.ViewModels
{
    public class LoginViewModel
{

    [Required]
    public string Full_name { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    
}
}


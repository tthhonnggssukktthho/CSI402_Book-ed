using System.ComponentModel.DataAnnotations;

namespace _66014444_Project.ViewModels.Admin;

public class AdminCustomerDetailViewModel
{
    public static readonly string[] StatusOptions = ["active", "suspended"];

    public int CustomerId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int CurrentPoints { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? ReceiverName { get; set; }
    public string? ReceiverPhone { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? Subdistrict { get; set; }
    public string? District { get; set; }
    public string? Province { get; set; }
    public string? PostalCode { get; set; }

    [Required]
    public string NewStatus { get; set; } = string.Empty;
}

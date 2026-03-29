namespace _66014444_Project.ViewModels.Admin;

public class AdminEmployeeListViewModel
{
    public List<AdminEmployeeItemViewModel> Employees { get; set; } = new();

    public string FilterStatus { get; set; } = "all";

    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class AdminEmployeeItemViewModel
{
    public int EmployeeId { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string EmploymentStatus { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; }
    public DateOnly? ResignDate { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Subdistrict { get; set; }
    public string? District { get; set; }
    public string? Province { get; set; }
    public string? PostalCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

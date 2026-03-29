namespace _66014444_Project.ViewModels.Admin;

public class AdminCustomerListViewModel
{
    public List<AdminCustomerItemViewModel> Customers { get; set; } = new();

    public string SearchKeyword { get; set; } = string.Empty;
    public string FilterStatus { get; set; } = "all";

    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class AdminCustomerItemViewModel
{
    public int CustomerId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public int CurrentPoints { get; set; }
    public DateTime CreatedAt { get; set; }
}

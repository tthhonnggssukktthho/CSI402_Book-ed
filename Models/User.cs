using System;
using System.Collections.Generic;

namespace _66014444_Project.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string UserType { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Employee> EmployeeCreatedByAdmins { get; set; } = new List<Employee>();

    public virtual Employee? EmployeeUser { get; set; }

    public virtual ICollection<PointTransaction> PointTransactions { get; set; } = new List<PointTransaction>();
}

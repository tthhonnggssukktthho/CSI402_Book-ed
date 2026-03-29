using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using _66014444_Project.Models;
using Microsoft.EntityFrameworkCore;
using _66014444_Project.ViewModels.Admin;

namespace _66014444_Project.Controllers;

[Authorize(Policy = "StaffOnly")]
public class AdminController : Controller
{
    private readonly _402block2Context _db;

    public AdminController(_402block2Context db)
    {
        _db = db;
    }

    [Authorize(Policy = "StaffOnly")]
    public IActionResult Dashboard()
    {
        return View();
    }

    [Authorize(Policy = "AdminOnly")]
    public IActionResult CustomerList()
    {
        var model = new AdminCustomerListViewModel
        {
            Customers = _db.Customers
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new AdminCustomerItemViewModel
                {
                    CustomerId = c.CustomerId,
                    Username = c.User.Username,
                    Email = c.User.Email,
                    DisplayName = c.DisplayName,
                    PhoneNumber = c.User.PhoneNumber,
                    Status = c.Status,
                    CurrentPoints = c.CurrentPoints,
                    CreatedAt = c.CreatedAt
                })
                .ToList()
        };

        return View(model);
    }

    [Authorize(Policy = "AdminOnly")]
    public IActionResult CustomerDetail(int id)
    {
        var customer = _db.Customers
            .Include(c => c.User)
            .FirstOrDefault(c => c.CustomerId == id);

        if (customer is null)
        {
            return NotFound();
        }

        var model = new AdminCustomerDetailViewModel
        {
            CustomerId = customer.CustomerId,
            Username = customer.User.Username,
            DisplayName = customer.DisplayName,
            Email = customer.User.Email,
            PhoneNumber = customer.User.PhoneNumber,
            BirthDate = customer.BirthDate,
            Status = customer.Status,
            CurrentPoints = customer.CurrentPoints,
            CreatedAt = customer.CreatedAt,
            IsActive = customer.User.IsActive,
            LastLoginAt = customer.User.LastLoginAt,
            ReceiverName = customer.ReceiverName,
            ReceiverPhone = customer.ReceiverPhone,
            AddressLine1 = customer.AddressLine1,
            AddressLine2 = customer.AddressLine2,
            Subdistrict = customer.Subdistrict,
            District = customer.District,
            Province = customer.Province,
            PostalCode = customer.PostalCode,
            NewStatus = customer.Status
        };

        return View(model);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateCustomerStatus(AdminCustomerDetailViewModel model)
    {
        var customer = _db.Customers
            .Include(c => c.User)
            .FirstOrDefault(c => c.CustomerId == model.CustomerId);

        if (customer is null)
        {
            return NotFound();
        }

        if (!AdminCustomerDetailViewModel.StatusOptions.Contains(model.NewStatus))
        {
            TempData["CustomerStatusError"] = "Invalid customer status.";
            return RedirectToAction(nameof(CustomerDetail), new { id = model.CustomerId });
        }

        customer.Status = model.NewStatus;
        customer.User.IsActive = model.NewStatus == "active";
        customer.User.UpdatedAt = DateTime.Now;
        _db.SaveChanges();

        TempData["CustomerStatusSuccess"] = "Customer status updated successfully.";
        return RedirectToAction(nameof(CustomerDetail), new { id = model.CustomerId });
    }

    [Authorize(Policy = "AdminOnly")]
    public IActionResult EmployeeList()
    {
        var model = new AdminEmployeeListViewModel
        {
            Employees = _db.Employees
                .Include(e => e.User)
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => new AdminEmployeeItemViewModel
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeCode = e.EmployeeCode,
                    FullName = e.FullName,
                    Role = e.Role,
                    EmploymentStatus = e.EmploymentStatus,
                    HireDate = e.HireDate,
                    ResignDate = e.ResignDate,
                    Username = e.User.Username,
                    Email = e.User.Email,
                    PhoneNumber = e.User.PhoneNumber,
                    Address1 = e.AddressLine1,
                    Address2 = e.AddressLine2,
                    Subdistrict = e.Subdistrict,
                    District = e.District,
                    Province = e.Province,
                    PostalCode = e.PostalCode,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt
                })
                .ToList()
        };

        return View(model);
    }

    [Authorize(Policy = "AdminOnly")]
    public IActionResult EmployeeCreate()
    {
        return View(new AdminEmployeeFormViewModel());
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ValidateAntiForgeryToken]
    public IActionResult EmployeeCreate(AdminEmployeeFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (_db.Users.Any(u => u.Username == model.Username))
        {
            ModelState.AddModelError(nameof(model.Username), "This username is already used.");
        }

        if (_db.Users.Any(u => u.Email == model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "This email is already used.");
        }

        if (!AdminEmployeeFormViewModel.EmployeeRoles.Contains(model.Role))
        {
            ModelState.AddModelError(nameof(model.Role), "Please select a valid role.");
        }

        if (!AdminEmployeeFormViewModel.EmploymentStatuses.Contains(model.EmploymentStatus))
        {
            ModelState.AddModelError(nameof(model.EmploymentStatus), "Please select a valid status.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var now = DateTime.Now;
        var employeeCode = GenerateEmployeeCode(model.Role);
        model.GeneratedEmployeeCode = employeeCode;

        var user = new User
        {
            Username = model.Username,
            Email = model.Email,
            PasswordHash = model.Password,
            PhoneNumber = model.PhoneNumber,
            UserType = "employee",
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.Users.Add(user);
        _db.SaveChanges();

        var employee = new Employee
        {
            UserId = user.UserId,
            EmployeeCode = employeeCode,
            FullName = model.FullName,
            Role = model.Role,
            HireDate = model.HireDate,
            EmploymentStatus = model.EmploymentStatus,
            AddressLine1 = model.Address1,
            AddressLine2 = model.Address2,
            Subdistrict = model.Subdistrict,
            District = model.District,
            Province = model.Province,
            PostalCode = model.PostalCode,
            ResignDate = model.EmploymentStatus == "resigned" ? model.ResignDate : null,
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.Employees.Add(employee);
        _db.SaveChanges();

        return RedirectToAction(nameof(EmployeeList));
    }

    [Authorize(Policy = "AdminOnly")]
    public IActionResult EmployeeEdit(int id)
    {
        var employee = _db.Employees
            .Include(e => e.User)
            .FirstOrDefault(e => e.EmployeeId == id);

        if (employee is null)
        {
            return NotFound();
        }

        var model = new AdminEmployeeFormViewModel
        {
            EmployeeId = employee.EmployeeId,
            Username = employee.User.Username,
            Email = employee.User.Email,
            FullName = employee.FullName,
            Role = employee.Role,
            EmployeeCode = employee.EmployeeCode,
            PhoneNumber = employee.User.PhoneNumber,
            Address1 = employee.AddressLine1,
            Address2 = employee.AddressLine2,
            Subdistrict = employee.Subdistrict,
            District = employee.District,
            Province = employee.Province,
            PostalCode = employee.PostalCode,
            HireDate = employee.HireDate,
            EmploymentStatus = employee.EmploymentStatus,
            ResignDate = employee.ResignDate
        };

        return View(model);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ValidateAntiForgeryToken]
    public IActionResult EmployeeEdit(AdminEmployeeFormViewModel model)
    {
        if (!model.EmployeeId.HasValue)
        {
            return BadRequest();
        }

        var employee = _db.Employees
            .Include(e => e.User)
            .FirstOrDefault(e => e.EmployeeId == model.EmployeeId.Value);

        if (employee is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.EmployeeCode = employee.EmployeeCode;
            return View(model);
        }

        if (_db.Users.Any(u => u.UserId != employee.UserId && u.Username == model.Username))
        {
            ModelState.AddModelError(nameof(model.Username), "This username is already used.");
        }

        if (_db.Users.Any(u => u.UserId != employee.UserId && u.Email == model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "This email is already used.");
        }

        if (!AdminEmployeeFormViewModel.EmployeeRoles.Contains(model.Role))
        {
            ModelState.AddModelError(nameof(model.Role), "Please select a valid role.");
        }

        if (!AdminEmployeeFormViewModel.EmploymentStatuses.Contains(model.EmploymentStatus))
        {
            ModelState.AddModelError(nameof(model.EmploymentStatus), "Please select a valid status.");
        }

        if (!ModelState.IsValid)
        {
            model.EmployeeCode = employee.EmployeeCode;
            return View(model);
        }

        var now = DateTime.Now;
        var roleChanged = !string.Equals(employee.Role, model.Role, StringComparison.OrdinalIgnoreCase);

        employee.User.Username = model.Username;
        employee.User.Email = model.Email;
        employee.User.PhoneNumber = model.PhoneNumber;
        employee.User.UpdatedAt = now;

        if (!string.IsNullOrWhiteSpace(model.Password))
        {
            employee.User.PasswordHash = model.Password;
        }

        employee.FullName = model.FullName;
        employee.Role = model.Role;
        employee.HireDate = model.HireDate;
        employee.EmploymentStatus = model.EmploymentStatus;
        employee.ResignDate = model.EmploymentStatus == "resigned" ? model.ResignDate : null;
        employee.AddressLine1 = model.Address1;
        employee.AddressLine2 = model.Address2;
        employee.Subdistrict = model.Subdistrict;
        employee.District = model.District;
        employee.Province = model.Province;
        employee.PostalCode = model.PostalCode;
        employee.UpdatedAt = now;

        if (roleChanged)
        {
            employee.EmployeeCode = GenerateEmployeeCode(model.Role, employee.EmployeeId);
        }

        _db.SaveChanges();

        return RedirectToAction(nameof(EmployeeList));
    }

    private string GenerateEmployeeCode(string role, int? excludeEmployeeId = null)
    {
        var prefix = role.ToLower() switch
        {
            "admin" => "ADM",
            "appraisal" => "APR",
            "finance" => "FIN",
            "shipping" => "SHP",
            _ => "EMP"
        };

        var latestCode = _db.Employees
            .Where(e => e.Role == role && (!excludeEmployeeId.HasValue || e.EmployeeId != excludeEmployeeId.Value))
            .OrderByDescending(e => e.EmployeeId)
            .Select(e => e.EmployeeCode)
            .FirstOrDefault();

        var nextNumber = 1;

        if (!string.IsNullOrWhiteSpace(latestCode))
        {
            var numericPart = latestCode.Length > prefix.Length
                ? latestCode[prefix.Length..]
                : string.Empty;

            if (int.TryParse(numericPart, out var currentNumber))
            {
                nextNumber = currentNumber + 1;
            }
        }

        return $"{prefix}{nextNumber:0000}";
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

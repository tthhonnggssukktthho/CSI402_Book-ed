using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using _66014444_Project.Models;
using Microsoft.EntityFrameworkCore;
using _66014444_Project.ViewModels.Admin;

namespace _66014444_Project.Controllers;

public class AdminController : Controller
{
    private readonly _402block2Context _db;

    public AdminController(_402block2Context db)
    {
        _db = db;
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult CustomerList()
    {
        var customers = _db.Customers
            .Include(c => c.User)
            .OrderByDescending(c => c.CreatedAt)
            .ToList();

        return View(customers);
    }

    public IActionResult CustomerDetails(int id)
    {
        // You can retrieve customer details based on the provided id and pass it to the view
        return View();
    }

    public IActionResult EmployeeList()
    {
        var employees = _db.Employees
            .Include(e => e.User)
            .OrderByDescending(e => e.CreatedAt)
            .ToList();

        return View(employees);
    }

    public IActionResult EmployeeCreate()
    {
        var model = new AdminEmployeeFormViewModel();
        return View(model);
    }

    [HttpPost]
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
            EmploymentStatus = "active",
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.Employees.Add(employee);
        _db.SaveChanges();

        return RedirectToAction(nameof(EmployeeList));
    }

    private string GenerateEmployeeCode(string role)
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
            .Where(e => e.Role == role)
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

    public IActionResult EmployeeEdit()
    {
        
        return View();
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

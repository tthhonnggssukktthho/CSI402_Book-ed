using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using _66014444_Project.Models;
using _66014444_Project.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace _66014444_Project.Controllers;

public class AccountController : Controller
{
    private readonly _402block2Context _db;
    public AccountController(_402block2Context db)
    {
        _db = db;
    }
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (_db.Users.Any(u => u.Username == model.Username))
        {
            ModelState.AddModelError(nameof(model.Username), "Username นี้ถูกใช้งานแล้ว");
        }

        if (_db.Users.Any(u => u.Email == model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "Email นี้ถูกใช้งานแล้ว");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var now = DateTime.Now;

        var user = new User
        {
            Username = model.Username,
            Email = model.Email,
            PasswordHash = model.Password,
            PhoneNumber = model.PhoneNumber,
            UserType = "customer",
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.Users.Add(user);
        _db.SaveChanges();

        var customer = new Customer
        {
            UserId = user.UserId,
            DisplayName = model.DisplayName,
            Status = "active",
            CurrentPoints = 0,
            CreatedAt = now
        };

        _db.Customers.Add(customer);
        _db.SaveChanges();

        return RedirectToAction("CustomerList", "Admin");
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

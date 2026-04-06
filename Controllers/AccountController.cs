using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _66014444_Project.Models;
using _66014444_Project.ViewModels;

namespace _66014444_Project.Controllers;

public class AccountController : Controller
{
    private readonly _402block2Context _db;

    public AccountController(_402block2Context db)
    {
        _db = db;
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public IActionResult Register(RegisterViewModel model)
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

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = _db.Users
            .Include(u => u.Customer)
            .Include(u => u.EmployeeUser)
            .FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == model.Password);

        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        if (user.IsActive != true)
        {
            ModelState.AddModelError(string.Empty, "This account is inactive.");
            return View(model);
        }

        var role = ResolveRole(user);
        var displayName = ResolveDisplayName(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, displayName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, role),
            new("user_type", user.UserType),
            new("username", user.Username)
        };

        if (user.EmployeeUser is not null)
        {
            claims.Add(new("employee_id", user.EmployeeUser.EmployeeId.ToString()));
            claims.Add(new("employee_code", user.EmployeeUser.EmployeeCode));
        }

        if (user.Customer is not null)
        {
            claims.Add(new("customer_id", user.Customer.CustomerId.ToString()));
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var properties = new AuthenticationProperties
        {
            IsPersistent = model.RememberMe,
            RedirectUri = model.ReturnUrl
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

        user.LastLoginAt = DateTime.Now;
        user.UpdatedAt = DateTime.Now;
        _db.SaveChanges();

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return LocalRedirect(model.ReturnUrl);
        }

        return RedirectToRoleHome(role);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "BookCatalog");
    }

    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            TempData["AccessDeniedMessage"] = "You do not have permission to open that page.";
            return RedirectToRoleHome(role);
        }

        return View();
    }

    private IActionResult RedirectToRoleHome(string? role)
    {
        return (role ?? string.Empty).ToLowerInvariant() switch
        {
            "customer" => RedirectToAction("Index", "BookCatalog"),
            "admin" => RedirectToAction("Dashboard", "Admin"),
            "appraisal" => RedirectToAction("Queue", "Appraisal"),
            "finance" => RedirectToAction("Queue", "Finance"),
            "shipping" => RedirectToAction("Queue", "Shipping"),
            _ => RedirectToAction(nameof(Login))
        };
    }

    private static string ResolveRole(User user)
    {
        if (string.Equals(user.UserType, "customer", StringComparison.OrdinalIgnoreCase))
        {
            return "customer";
        }

        return user.EmployeeUser?.Role?.ToLowerInvariant() ?? "employee";
    }

    private static string ResolveDisplayName(User user)
    {
        if (string.Equals(user.UserType, "customer", StringComparison.OrdinalIgnoreCase))
        {
            return user.Customer?.DisplayName ?? user.Username;
        }

        return user.EmployeeUser?.FullName ?? user.Username;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

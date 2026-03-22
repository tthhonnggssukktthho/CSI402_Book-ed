using  System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using _66014444_Project.Models;

namespace _66014444_Project.Controllers;

public class AccountController : Controller
{
    public IActionResult Register()
    {
        return View();
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
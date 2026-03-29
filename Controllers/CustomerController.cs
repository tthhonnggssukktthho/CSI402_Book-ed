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

[Authorize(Roles = "customer")]
public class CustomerController : Controller
{
    private readonly _402block2Context _db;

    public CustomerController(_402block2Context db)
    {
        _db = db;
    }

    public IActionResult Profile()
    {
        return View();
    }

    public IActionResult MyBooks()
    {
        return View();
    }
}

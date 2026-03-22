using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using _66014444_Project.Models;

namespace _66014444_Project.Controllers;

public class AdminController : Controller
{
    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult CustomerList()
    {
        return View();
    }

    public IActionResult CustomerDetails(int id)
    {
        // You can retrieve customer details based on the provided id and pass it to the view
        return View();
    }

    public IActionResult EmployeeList()
    {
        return View();
    }

    public IActionResult EmployeeCreate()
    {
        return View();
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

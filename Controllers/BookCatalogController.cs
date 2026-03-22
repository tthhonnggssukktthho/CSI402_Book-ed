using Microsoft.AspNetCore.Mvc;

namespace _66014444_Project.Controllers;

public class BookCatalogController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Book Catalog";
        ViewData["ActiveCategory"] = "All";
        return View();
    }
}

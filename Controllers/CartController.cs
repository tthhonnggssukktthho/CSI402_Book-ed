using Microsoft.AspNetCore.Mvc;
using _66014444_Project.Models;
using _66014444_Project.ViewModels.BookCatalog;

namespace _66014444_Project.Controllers;

public class CartController : Controller
{
    private readonly _402block2Context _db;
    public CartController(_402block2Context db)
    {
        _db = db;
    }

    public IActionResult CartIndex()
    {
        View();
    }

}
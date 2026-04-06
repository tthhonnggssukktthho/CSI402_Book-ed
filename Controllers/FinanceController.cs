using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _66014444_Project.Controllers;

[Authorize(Roles = "admin,finance")]
public class FinanceController : Controller
{
    public IActionResult Queue()
    {
        return View();
    }
}

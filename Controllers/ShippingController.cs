using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _66014444_Project.Controllers;

[Authorize(Roles = "admin,shipping")]
public class ShippingController : Controller
{
    public IActionResult Queue()
    {
        return View();
    }
}

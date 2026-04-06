using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _66014444_Project.Controllers;

[Authorize(Roles = "customer")]
public class PaymentController : Controller
{
    public IActionResult Index()
    {
        return Content("Payment page is not created yet.");
    }
}

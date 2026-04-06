using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using _66014444_Project.Models;
using _66014444_Project.ViewModels;

namespace _66014444_Project.Controllers;

[Authorize(Roles = "admin,appraisal")]
public class AppraisalController : Controller
{
    private readonly _402block2Context _db;

    public AppraisalController(_402block2Context db)
    {
        _db = db;
    }

    public IActionResult Queue()
    {
        return View();
    }

    public IActionResult Review()
    {
        return View();
    }
}

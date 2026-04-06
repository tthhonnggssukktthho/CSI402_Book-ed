using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using _66014444_Project.Models;
using _66014444_Project.ViewModels.SellBook;

namespace _66014444_Project.Controllers;

[Authorize(Roles = "customer")]
public class SellBookController : Controller
{
    private readonly _402block2Context _db;
    private readonly IWebHostEnvironment _environment;

    public SellBookController(_402block2Context db, IWebHostEnvironment environment)
    {
        _db = db;
        _environment = environment;
    }

    public IActionResult Create()
    {
        return View(BuildCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SellBookCreateViewModel model)
    {
        PopulateOptions(model);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Challenge();
        }

        var customer = _db.Customers.FirstOrDefault(c => c.UserId == userId);
        if (customer is null)
        {
            return NotFound();
        }

        var uploadDirectory = Path.Combine(_environment.WebRootPath, "uploads", "books");
        Directory.CreateDirectory(uploadDirectory);

        var imagePath1 = await SaveImageAsync(model.ImageFile1!, uploadDirectory);
        var imagePath2 = await SaveImageAsync(model.ImageFile2!, uploadDirectory);
        var imagePath3 = await SaveImageAsync(model.ImageFile3!, uploadDirectory);
        var imagePath4 = await SaveImageAsync(model.ImageFile4!, uploadDirectory);

        var now = DateTime.Now;

        var book = new Book
        {
            Title = model.Title,
            SeriesName = model.SeriesName,
            VolumeNo = model.VolumeNo,
            CategoryName = model.CategoryName,
            PublisherName = model.PublisherName,
            Synopsis = model.Synopsis,
            BookDescription = model.BookDescription,
            ImageUrl = imagePath1,
            ImageUrl2 = imagePath2,
            ImageUrl3 = imagePath3,
            ImageUrl4 = imagePath4,
            ConditionCode = model.ConditionCode,
            ConditionDiscountPct = 0,
            ConditionNote = model.ConditionNote,
            ProposedPrice = model.ProposedPrice,
            ApprovalStatus = "pending",
            SaleStatus = "draft",
            SellerCustomerId = customer.CustomerId,
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.Books.Add(book);
        _db.SaveChanges();

        TempData["SellBookSuccess"] = "ส่งข้อมูลหนังสือเข้าระบบเรียบร้อยแล้ว";
        return RedirectToAction("MyBooks", "Customer");
    }

    public IActionResult Edit()
    {
        return View();
    }

    private SellBookCreateViewModel BuildCreateViewModel()
    {
        var model = new SellBookCreateViewModel();
        PopulateOptions(model);
        return model;
    }

    private static void PopulateOptions(SellBookCreateViewModel model)
    {
        model.Categories =
        [
            "โรแมนซ์",
            "สยองขวัญ",
            "แฟนตาซี",
            "วาย/ยูริ",
            "สืบสวน"
        ];

        model.ConditionOptions =
        [
            new SelectListItem("เหมือนใหม่", "LIKE_NEW"),
            new SelectListItem("สภาพดี", "GOOD"),
            new SelectListItem("มีตำหนิเล็กน้อย", "MINOR_DEFECT"),
            new SelectListItem("มีตำหนิชัดเจน", "MAJOR_DEFECT")
        ];
    }

    private static async Task<string> SaveImageAsync(IFormFile file, string uploadDirectory)
    {
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadDirectory, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/uploads/books/{fileName}";
    }
}

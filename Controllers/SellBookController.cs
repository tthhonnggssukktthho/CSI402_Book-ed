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

        var customer = GetCurrentCustomer();
        if (customer is null)
        {
            return Challenge();
        }

        var uploadDirectory = GetUploadDirectory();

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
            AuthorName = model.AuthorName,
            Isbn = model.Isbn,
            PublishYear = model.PublishYear,
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

    public IActionResult Edit(int id)
    {
        var customer = GetCurrentCustomer();
        if (customer is null)
        {
            return Challenge();
        }

        var book = _db.Books.FirstOrDefault(b => b.BookId == id && b.SellerCustomerId == customer.CustomerId);
        if (book is null)
        {
            return NotFound();
        }

        if (!CanEditBook(book))
        {
            TempData["SellBookError"] = "หนังสือรายการนี้ไม่สามารถแก้ไขได้แล้ว";
            return RedirectToAction("MyBooks", "Customer");
        }

        return View(BuildEditViewModel(book));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SellBookEditViewModel model)
    {
        PopulateOptions(model);

        var customer = GetCurrentCustomer();
        if (customer is null)
        {
            return Challenge();
        }

        var book = _db.Books.FirstOrDefault(b => b.BookId == model.BookId && b.SellerCustomerId == customer.CustomerId);
        if (book is null)
        {
            return NotFound();
        }

        if (!CanEditBook(book))
        {
            TempData["SellBookError"] = "หนังสือรายการนี้ไม่สามารถแก้ไขได้แล้ว";
            return RedirectToAction("MyBooks", "Customer");
        }

        model.ExistingImageUrl1 = book.ImageUrl;
        model.ExistingImageUrl2 = book.ImageUrl2;
        model.ExistingImageUrl3 = book.ImageUrl3;
        model.ExistingImageUrl4 = book.ImageUrl4;
        model.ApprovalStatus = book.ApprovalStatus;
        model.SaleStatus = book.SaleStatus;
        model.RejectionReason = book.RejectionReason;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var uploadDirectory = GetUploadDirectory();

        book.Title = model.Title;
        book.SeriesName = model.SeriesName;
        book.VolumeNo = model.VolumeNo;
        book.CategoryName = model.CategoryName;
        book.PublisherName = model.PublisherName;
        book.AuthorName = model.AuthorName;
        book.Isbn = model.Isbn;
        book.PublishYear = model.PublishYear;
        book.Synopsis = model.Synopsis;
        book.BookDescription = model.BookDescription;
        book.ImageUrl = await SaveImageOrKeepAsync(model.ImageFile1, book.ImageUrl, uploadDirectory);
        book.ImageUrl2 = await SaveImageOrKeepAsync(model.ImageFile2, book.ImageUrl2, uploadDirectory);
        book.ImageUrl3 = await SaveImageOrKeepAsync(model.ImageFile3, book.ImageUrl3, uploadDirectory);
        book.ImageUrl4 = await SaveImageOrKeepAsync(model.ImageFile4, book.ImageUrl4, uploadDirectory);
        book.ConditionCode = model.ConditionCode;
        book.ConditionNote = model.ConditionNote;
        book.ProposedPrice = model.ProposedPrice;
        book.UpdatedAt = DateTime.Now;

        if (book.ApprovalStatus == "rejected")
        {
            book.ApprovalStatus = "pending";
            book.RejectionReason = null;
        }

        _db.SaveChanges();

        TempData["SellBookSuccess"] = "อัปเดตข้อมูลหนังสือเรียบร้อยแล้ว";
        return RedirectToAction("MyBooks", "Customer");
    }

    private SellBookCreateViewModel BuildCreateViewModel()
    {
        var model = new SellBookCreateViewModel();
        PopulateOptions(model);
        return model;
    }

    private SellBookEditViewModel BuildEditViewModel(Book book)
    {
        var model = new SellBookEditViewModel
        {
            BookId = book.BookId,
            Title = book.Title,
            SeriesName = book.SeriesName,
            VolumeNo = book.VolumeNo,
            CategoryName = book.CategoryName,
            PublisherName = book.PublisherName,
            AuthorName = book.AuthorName,
            Isbn = book.Isbn,
            PublishYear = book.PublishYear,
            Synopsis = book.Synopsis,
            BookDescription = book.BookDescription,
            ExistingImageUrl1 = book.ImageUrl,
            ExistingImageUrl2 = book.ImageUrl2,
            ExistingImageUrl3 = book.ImageUrl3,
            ExistingImageUrl4 = book.ImageUrl4,
            ConditionCode = book.ConditionCode,
            ConditionNote = book.ConditionNote ?? string.Empty,
            ProposedPrice = book.ProposedPrice,
            ApprovalStatus = book.ApprovalStatus,
            SaleStatus = book.SaleStatus,
            RejectionReason = book.RejectionReason
        };

        PopulateOptions(model);
        return model;
    }

    private Customer? GetCurrentCustomer()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return null;
        }

        return _db.Customers.FirstOrDefault(c => c.UserId == userId);
    }

    private string GetUploadDirectory()
    {
        var uploadDirectory = Path.Combine(_environment.WebRootPath, "uploads", "books");
        Directory.CreateDirectory(uploadDirectory);
        return uploadDirectory;
    }

    private static bool CanEditBook(Book book)
    {
        return book.ApprovalStatus == "pending" || book.ApprovalStatus == "rejected";
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

    private static void PopulateOptions(SellBookEditViewModel model)
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

    private static async Task<string?> SaveImageOrKeepAsync(IFormFile? file, string? existingPath, string uploadDirectory)
    {
        if (file is null)
        {
            return existingPath;
        }

        return await SaveImageAsync(file, uploadDirectory);
    }
}

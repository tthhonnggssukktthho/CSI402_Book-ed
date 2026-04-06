using Microsoft.AspNetCore.Mvc;
using _66014444_Project.Models;
using _66014444_Project.ViewModels.BookCatalog;

namespace _66014444_Project.Controllers;

public class BookCatalogController : Controller
{
    private readonly _402block2Context _db;

    public BookCatalogController(_402block2Context db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Book Catalog";
        ViewData["ActiveCategory"] = "All";

        var approvedBooks = _db.Books
            .Where(b => b.ApprovalStatus == "approved")
            .OrderByDescending(b => b.UpdatedAt)
            .ToList();

        var model = new BookCatalogIndexViewModel
        {
            TotalItems = approvedBooks.Count,
            TotalPages = approvedBooks.Count == 0 ? 1 : (int)Math.Ceiling(approvedBooks.Count / 12m),
            Categories = approvedBooks
                .Select(b => b.CategoryName)
                .Where(category => !string.IsNullOrWhiteSpace(category))
                .Distinct()
                .OrderBy(category => category)
                .ToList(),
            Books = approvedBooks
                .Select(b => new BookCardViewModel
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    SeriesName = b.SeriesName,
                    VolumeNo = b.VolumeNo,
                    AuthorName = b.AuthorName,
                    CategoryName = b.CategoryName,
                    ImageUrl = b.ImageUrl,
                    ConditionCode = b.ConditionCode,
                    ConditionDiscountPct = b.ConditionDiscountPct,
                    ApprovedPrice = b.ApprovedPrice ?? 0m,
                    FinalPrice = (b.ApprovedPrice ?? 0m) * (1 - (b.ConditionDiscountPct / 100m)),
                    ConditionNote = b.ConditionNote
                })
                .ToList()
        };

        return View(model);
    }
}

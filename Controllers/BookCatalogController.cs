using Microsoft.AspNetCore.Mvc;
using _66014444_Project.Models;
using _66014444_Project.Services;
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
                .Select(MapBookCard)
                .ToList()
        };

        return View(model);
    }

    public IActionResult Detail(int id)
    {
        var book = _db.Books.FirstOrDefault(b => b.BookId == id && b.ApprovalStatus == "approved");
        if (book is null)
        {
            return NotFound();
        }

        var approvedPrice = book.ApprovedPrice ?? 0m;
        var conditionDiscountPct = ConditionDiscountHelper.ResolvePercent(book.ConditionCode, book.ConditionDiscountPct);
        var model = new BookDetailViewModel
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
            ImageUrl = book.ImageUrl,
            ImageUrl2 = book.ImageUrl2,
            ImageUrl3 = book.ImageUrl3,
            ImageUrl4 = book.ImageUrl4,
            ConditionCode = book.ConditionCode,
            ConditionDiscountPct = conditionDiscountPct,
            ConditionNote = book.ConditionNote,
            ApprovedPrice = approvedPrice,
            FinalPrice = approvedPrice * (1 - (conditionDiscountPct / 100m)),
            SaleStatus = book.SaleStatus,
            CanAddToCart = !string.Equals(book.SaleStatus, "sold", StringComparison.OrdinalIgnoreCase)
        };

        return View(model);
    }

    private static BookCardViewModel MapBookCard(Book book)
    {
        var approvedPrice = book.ApprovedPrice ?? 0m;
        var conditionDiscountPct = ConditionDiscountHelper.ResolvePercent(book.ConditionCode, book.ConditionDiscountPct);

        return new BookCardViewModel
        {
            BookId = book.BookId,
            Title = book.Title,
            SeriesName = book.SeriesName,
            VolumeNo = book.VolumeNo,
            AuthorName = book.AuthorName,
            CategoryName = book.CategoryName,
            ImageUrl = book.ImageUrl,
            ConditionCode = book.ConditionCode,
            ConditionDiscountPct = conditionDiscountPct,
            ApprovedPrice = approvedPrice,
            FinalPrice = approvedPrice * (1 - (conditionDiscountPct / 100m)),
            ConditionNote = book.ConditionNote
        };
    }
}

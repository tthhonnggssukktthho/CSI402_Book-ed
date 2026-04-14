using Microsoft.AspNetCore.Mvc;
using _66014444_Project.Models;
using _66014444_Project.Services;
using _66014444_Project.ViewModels.BookCatalog;

namespace _66014444_Project.Controllers;

public class BookCatalogController : Controller
{
    private readonly _402block2Context _db;

    private static readonly string[] SellableStatuses = ["ready_for_sale", "draft"];

    public BookCatalogController(_402block2Context db)
    {
        _db = db;
    }

    public IActionResult Index(
        string? searchKeyword,
        string? searchType,
        string? category,
        string? condition,
        string? priceRange,
        string? sortBy)
    {
        searchKeyword = (searchKeyword ?? string.Empty).Trim();
        searchType = NormalizeSearchType(searchType);
        sortBy = NormalizeSortBy(sortBy);
        category = NormalizeOptionalValue(category);
        condition = NormalizeOptionalValue(condition);
        priceRange = NormalizeOptionalValue(priceRange);

        ViewData["Title"] = "Book Catalog";
        ViewData["ActiveCategory"] = category ?? "All";
        ViewData["SearchType"] = searchType;
        ViewData["SearchKeyword"] = searchKeyword;

        var sellableBooks = _db.Books
            .Where(b => b.ApprovalStatus == "approved" && SellableStatuses.Contains(b.SaleStatus))
            .OrderByDescending(b => b.UpdatedAt)
            .ToList();

        var categories = sellableBooks
            .Select(b => b.CategoryName)
            .Where(categoryName => !string.IsNullOrWhiteSpace(categoryName))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(categoryName => categoryName)
            .ToList();

        var filteredCards = sellableBooks
            .Select(MapBookCard)
            .Where(card => MatchesSearch(card, searchType, searchKeyword))
            .Where(card => string.IsNullOrWhiteSpace(category) || string.Equals(card.CategoryName, category, StringComparison.OrdinalIgnoreCase))
            .Where(card => string.IsNullOrWhiteSpace(condition) || string.Equals(card.ConditionCode, condition, StringComparison.OrdinalIgnoreCase))
            .Where(card => MatchesPriceRange(card.FinalPrice, priceRange))
            .ToList();

        filteredCards = ApplySort(filteredCards, sortBy);

        var model = new BookCatalogIndexViewModel
        {
            SearchKeyword = searchKeyword,
            SearchType = searchType,
            FilterCategory = category,
            FilterCondition = condition,
            PriceRange = priceRange,
            SortBy = sortBy,
            TotalItems = filteredCards.Count,
            TotalPages = filteredCards.Count == 0 ? 1 : (int)Math.Ceiling(filteredCards.Count / 12m),
            Categories = categories,
            Books = filteredCards
        };

        return View(model);
    }

    public IActionResult Detail(int id)
    {
        var book = _db.Books.FirstOrDefault(b => b.BookId == id && b.ApprovalStatus == "approved" && SellableStatuses.Contains(b.SaleStatus));
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
            CanAddToCart = SellableStatuses.Contains(book.SaleStatus)
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
            Isbn = book.Isbn,
            CategoryName = book.CategoryName,
            ImageUrl = book.ImageUrl,
            ConditionCode = book.ConditionCode,
            ConditionDiscountPct = conditionDiscountPct,
            ApprovedPrice = approvedPrice,
            FinalPrice = approvedPrice * (1 - (conditionDiscountPct / 100m)),
            ConditionNote = book.ConditionNote
        };
    }

    private static string NormalizeSearchType(string? searchType)
    {
        return (searchType ?? string.Empty).Trim().ToLowerInvariant() switch
        {
            "author" => "author",
            "series" => "series",
            "isbn" => "isbn",
            _ => "title"
        };
    }

    private static string NormalizeSortBy(string? sortBy)
    {
        return (sortBy ?? string.Empty).Trim().ToLowerInvariant() switch
        {
            "price_asc" => "price_asc",
            "price_desc" => "price_desc",
            "latest" => "latest",
            _ => "recommended"
        };
    }

    private static string? NormalizeOptionalValue(string? value)
    {
        var normalized = (value ?? string.Empty).Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }

    private static bool MatchesSearch(BookCardViewModel card, string searchType, string searchKeyword)
    {
        if (string.IsNullOrWhiteSpace(searchKeyword))
        {
            return true;
        }

        var keyword = searchKeyword.Trim();

        return searchType switch
        {
            "author" => (card.AuthorName ?? string.Empty).Contains(keyword, StringComparison.OrdinalIgnoreCase),
            "series" => (card.SeriesName ?? string.Empty).Contains(keyword, StringComparison.OrdinalIgnoreCase),
            "isbn" => (card.Isbn ?? string.Empty).Contains(keyword, StringComparison.OrdinalIgnoreCase),
            _ => (card.Title ?? string.Empty).Contains(keyword, StringComparison.OrdinalIgnoreCase)
        };
    }

    private static bool MatchesPriceRange(decimal finalPrice, string? priceRange)
    {
        return priceRange switch
        {
            "under_200" => finalPrice < 200m,
            "200_300" => finalPrice >= 200m && finalPrice <= 300m,
            "over_300" => finalPrice > 300m,
            _ => true
        };
    }

    private static List<BookCardViewModel> ApplySort(List<BookCardViewModel> cards, string sortBy)
    {
        return sortBy switch
        {
            "price_asc" => cards.OrderBy(card => card.FinalPrice).ThenBy(card => card.Title).ToList(),
            "price_desc" => cards.OrderByDescending(card => card.FinalPrice).ThenBy(card => card.Title).ToList(),
            "latest" => cards.OrderByDescending(card => card.BookId).ToList(),
            _ => cards.OrderByDescending(card => card.BookId).ToList()
        };
    }
}

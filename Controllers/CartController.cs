using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _66014444_Project.Models;
using _66014444_Project.ViewModels.Cart;

namespace _66014444_Project.Controllers;

public class CartController : Controller
{
    private readonly _402block2Context _db;

    public CartController(_402block2Context db)
    {
        _db = db;
    }

    [Authorize(Roles = "customer")]
    public IActionResult CartIndex()
    {
        var customerIdClaim = User.FindFirst("customer_id")?.Value;
        if (!int.TryParse(customerIdClaim, out var customerId))
        {
            return Challenge();
        }

        var items = _db.CartItems
            .Include(c => c.Book)
            .Where(c => c.CustomerId == customerId)
            .OrderByDescending(c => c.AddedAt)
            .ToList();

        var model = new CartViewModel
        {
            Items = items
                .Select(item => new CartItemViewModel
                {
                    CartItemId = item.CartItemId,
                    BookId = item.BookId,
                    Title = item.Book.Title,
                    SeriesName = item.Book.SeriesName,
                    VolumeNo = item.Book.VolumeNo,
                    ImageUrl = item.Book.ImageUrl,
                    ConditionCode = item.Book.ConditionCode,
                    UnitPrice = item.UnitPrice,
                    ConditionDiscountPct = item.Book.ConditionDiscountPct,
                    FinalPrice = item.UnitPrice * (1 - (item.Book.ConditionDiscountPct / 100m)),
                    SaleStatus = item.Book.SaleStatus,
                    IsAvailable = !string.Equals(item.Book.SaleStatus, "sold", StringComparison.OrdinalIgnoreCase),
                    AddedAt = item.AddedAt
                })
                .ToList()
        };

        model.Subtotal = model.Items.Sum(item => item.FinalPrice);

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "customer")]
    [ValidateAntiForgeryToken]
    public IActionResult Add(int bookId, string? returnUrl = null)
    {
        var customerIdClaim = User.FindFirst("customer_id")?.Value;
        if (!int.TryParse(customerIdClaim, out var customerId))
        {
            return Challenge();
        }

        var book = _db.Books.FirstOrDefault(b => b.BookId == bookId && b.ApprovalStatus == "approved");
        if (book is null)
        {
            return NotFound();
        }

        var alreadyInCart = _db.CartItems.Any(c => c.CustomerId == customerId && c.BookId == bookId);
        if (!alreadyInCart)
        {
            _db.CartItems.Add(new CartItem
            {
                CustomerId = customerId,
                BookId = book.BookId,
                UnitPrice = book.ApprovedPrice ?? 0m,

            });
            _db.SaveChanges();
            TempData["CartSuccess"] = "เพิ่มหนังสือลงตะกร้าเรียบร้อยแล้ว";
        }
        else
        {
            TempData["CartSuccess"] = "หนังสือเล่มนี้อยู่ในตะกร้าแล้ว";
        }

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToAction(nameof(CartIndex));
    }

    [HttpPost]
    [Authorize(Roles = "customer")]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveFromCart(int itemId)
    {
        var customerIdClaim = User.FindFirst("customer_id")?.Value;
        if (!int.TryParse(customerIdClaim, out var customerId))
        {
            return Challenge();
        }

        var cartItem = _db.CartItems.FirstOrDefault(c => c.CartItemId == itemId && c.CustomerId == customerId);
        if (cartItem is null)
        {
            return NotFound();
        }

        _db.CartItems.Remove(cartItem);
        _db.SaveChanges();

        TempData["CartSuccess"] = "ลบหนังสือออกจากตะกร้าเรียบร้อยแล้ว";
        return RedirectToAction(nameof(CartIndex));
    }
    
}

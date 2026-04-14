using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _66014444_Project.Models;
using _66014444_Project.Services;
using _66014444_Project.ViewModels.Cart;

namespace _66014444_Project.Controllers;

public class CartController : Controller
{
    private readonly _402block2Context _db;

    private static readonly string[] SellableStatuses = ["ready_for_sale", "draft"];

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
                .Select(item =>
                {
                    var conditionDiscountPct = ConditionDiscountHelper.ResolvePercent(item.Book.ConditionCode, item.Book.ConditionDiscountPct);
                    return new CartItemViewModel
                    {
                        CartItemId = item.CartItemId,
                        BookId = item.BookId,
                        Title = item.Book.Title,
                        SeriesName = item.Book.SeriesName,
                        VolumeNo = item.Book.VolumeNo,
                        ImageUrl = item.Book.ImageUrl,
                        ConditionCode = item.Book.ConditionCode,
                        UnitPrice = item.UnitPrice,
                        ConditionDiscountPct = conditionDiscountPct,
                        FinalPrice = item.UnitPrice * (1 - (conditionDiscountPct / 100m)),
                        SaleStatus = item.Book.SaleStatus,
                        IsAvailable = SellableStatuses.Contains(item.Book.SaleStatus),
                        AddedAt = item.AddedAt
                    };
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

        var book = _db.Books.FirstOrDefault(b => b.BookId == bookId && b.ApprovalStatus == "approved" && SellableStatuses.Contains(b.SaleStatus));
        if (book is null)
        {
            TempData["CartSuccess"] = "หนังสือเล่มนี้ไม่พร้อมขายแล้ว";
            return RedirectToAction("Index", "BookCatalog");
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

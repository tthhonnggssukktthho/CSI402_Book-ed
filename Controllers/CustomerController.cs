using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _66014444_Project.Models;
using _66014444_Project.ViewModels.Customer;

namespace _66014444_Project.Controllers;

[Authorize(Roles = "customer")]
public class CustomerController : Controller
{
    private readonly _402block2Context _db;

    public CustomerController(_402block2Context db)
    {
        _db = db;
    }

    public IActionResult Profile()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Challenge();
        }

        var customer = _db.Customers
            .Include(c => c.User)
            .FirstOrDefault(c => c.UserId == userId);

        if (customer is null)
        {
            return NotFound();
        }

        var model = new ProfileViewModel
        {
            CustomerId = customer.CustomerId,
            DisplayName = customer.DisplayName,
            Email = customer.User.Email,
            PhoneNumber = customer.User.PhoneNumber,
            BirthDate = customer.BirthDate,
            Status = customer.Status,
            ReceiverName = customer.ReceiverName,
            ReceiverPhone = customer.ReceiverPhone,
            AddressLine1 = customer.AddressLine1,
            AddressLine2 = customer.AddressLine2,
            Subdistrict = customer.Subdistrict,
            District = customer.District,
            Province = customer.Province,
            PostalCode = customer.PostalCode,
            CurrentPoints = customer.CurrentPoints,
            CreatedAt = customer.CreatedAt
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Profile(ProfileViewModel model)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Challenge();
        }

        var customer = _db.Customers
            .Include(c => c.User)
            .FirstOrDefault(c => c.UserId == userId);

        if (customer is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.CurrentPoints = customer.CurrentPoints;
            model.CreatedAt = customer.CreatedAt;
            model.Status = customer.Status;
            return View(model);
        }

        if (_db.Users.Any(u => u.UserId != customer.UserId && u.Email == model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "This email is already used.");
        }

        if (!ModelState.IsValid)
        {
            model.CurrentPoints = customer.CurrentPoints;
            model.CreatedAt = customer.CreatedAt;
            model.Status = customer.Status;
            return View(model);
        }

        customer.DisplayName = model.DisplayName;
        customer.BirthDate = model.BirthDate;
        customer.ReceiverName = model.ReceiverName;
        customer.ReceiverPhone = model.ReceiverPhone;
        customer.AddressLine1 = model.AddressLine1;
        customer.AddressLine2 = model.AddressLine2;
        customer.Subdistrict = model.Subdistrict;
        customer.District = model.District;
        customer.Province = model.Province;
        customer.PostalCode = model.PostalCode;

        customer.User.Email = model.Email;
        customer.User.PhoneNumber = model.PhoneNumber;
        customer.User.UpdatedAt = DateTime.Now;

        _db.SaveChanges();

        return RedirectToAction(nameof(Profile));
    }

    public IActionResult MyBooks()
    {
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

        var model = new MyBooksViewModel
        {
            SellingBooks = _db.Books
                .Where(b => b.SellerCustomerId == customer.CustomerId)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new MyBookItemViewModel
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    SeriesName = b.SeriesName,
                    VolumeNo = b.VolumeNo,
                    ConditionCode = b.ConditionCode,
                    ConditionNote = b.ConditionNote,
                    ProposedPrice = b.ProposedPrice,
                    ApprovedPrice = b.ApprovedPrice,
                    ApprovalStatus = b.ApprovalStatus,
                    SaleStatus = b.SaleStatus,
                    ImageUrl = b.ImageUrl,
                    CreatedAt = b.CreatedAt,
                    CanEdit = b.ApprovalStatus == "pending" || b.ApprovalStatus == "rejected"
                })
                .ToList()
        };

        return View(model);
    }
}

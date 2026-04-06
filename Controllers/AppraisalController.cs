using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using _66014444_Project.Models;
using Microsoft.EntityFrameworkCore;
using _66014444_Project.ViewModels.Appraisal;

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
        var model = new AppraisalQueueViewModel
        {
            Books = _db.Books
                .Include(b => b.SellerCustomer)
                .ThenInclude(c => c.User)
                .OrderBy(b => b.ApprovalStatus == "pending" ? 0 : 1)
                .ThenByDescending(b => b.CreatedAt)
                .Select(b => new AppraisalQueueItemViewModel
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    SeriesName = b.SeriesName,
                    VolumeNo = b.VolumeNo,
                    CategoryName = b.CategoryName,
                    PublisherName = b.PublisherName,
                    AuthorName = b.AuthorName,
                    SellerDisplayName = b.SellerCustomer.DisplayName,
                    SellerUsername = b.SellerCustomer.User.Username,
                    ConditionCode = b.ConditionCode,
                    ConditionNote = b.ConditionNote,
                    ProposedPrice = b.ProposedPrice,
                    ApprovedPrice = b.ApprovedPrice,
                    ApprovalStatus = b.ApprovalStatus,
                    SaleStatus = b.SaleStatus,
                    ImageUrl = b.ImageUrl,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt
                })
                .ToList()
        };

        return View(model);
    }

    public IActionResult Review()
    {
        return View();
    }
}

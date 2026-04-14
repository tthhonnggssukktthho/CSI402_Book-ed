using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _66014444_Project.Models;
using _66014444_Project.Services;
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

    public IActionResult Review(int id)
    {
        var book = LoadBookForReview(id);
        if (book is null)
        {
            return NotFound();
        }

        return View(BuildReviewViewModel(book));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Review(AppraisalReviewViewModel model, string submitAction)
    {
        PopulateOptions(model);

        var book = LoadBookForReview(model.BookId);
        if (book is null)
        {
            return NotFound();
        }

        ApplyEditableFields(book, model);

        var reviewerEmployeeId = GetCurrentEmployeeId();
        var reviewerName = User.Identity?.Name;

        if (string.Equals(submitAction, "approve", StringComparison.OrdinalIgnoreCase))
        {
            if (!model.ApprovedPrice.HasValue || model.ApprovedPrice <= 0)
            {
                ModelState.AddModelError(nameof(model.ApprovedPrice), "กรุณาระบุราคาที่อนุมัติ");
            }
        }

        if (string.Equals(submitAction, "reject", StringComparison.OrdinalIgnoreCase) &&
            string.IsNullOrWhiteSpace(model.RejectionReason))
        {
            ModelState.AddModelError(nameof(model.RejectionReason), "กรุณาระบุเหตุผลที่ไม่อนุมัติ");
        }

        if (!ModelState.IsValid)
        {
            model.SellerDisplayName = book.SellerCustomer.DisplayName;
            model.SellerUsername = book.SellerCustomer.User.Username;
            model.ImageUrl = book.ImageUrl;
            model.ImageUrl2 = book.ImageUrl2;
            model.ImageUrl3 = book.ImageUrl3;
            model.ImageUrl4 = book.ImageUrl4;
            model.ApprovalStatus = book.ApprovalStatus;
            model.SaleStatus = book.SaleStatus;
            model.ReviewedByName = book.ReviewedByEmployee?.FullName ?? reviewerName;
            model.CreatedAt = book.CreatedAt;
            model.UpdatedAt = book.UpdatedAt;
            model.ReviewedAt = book.ReviewedAt;
            return View(model);
        }

        book.UpdatedAt = DateTime.Now;

        switch ((submitAction ?? string.Empty).ToLowerInvariant())
        {
            case "approve":
                book.ApprovalStatus = "approved";
                book.ApprovedPrice = model.ApprovedPrice;
                book.RejectionReason = null;
                book.ReviewedAt = DateTime.Now;
                if (reviewerEmployeeId.HasValue)
                {
                    book.ReviewedByEmployeeId = reviewerEmployeeId.Value;
                }
                TempData["AppraisalSuccess"] = "อนุมัติรายการหนังสือเรียบร้อยแล้ว";
                break;

            case "reject":
                book.ApprovalStatus = "rejected";
                book.RejectionReason = model.RejectionReason?.Trim();
                book.ReviewedAt = DateTime.Now;
                if (reviewerEmployeeId.HasValue)
                {
                    book.ReviewedByEmployeeId = reviewerEmployeeId.Value;
                }
                TempData["AppraisalSuccess"] = "บันทึกผลไม่อนุมัติเรียบร้อยแล้ว";
                break;

            default:
                TempData["AppraisalSuccess"] = "บันทึกการแก้ไขข้อมูลเรียบร้อยแล้ว";
                break;
        }

        _db.SaveChanges();
        return RedirectToAction(nameof(Review), new { id = model.BookId });
    }

    private Book? LoadBookForReview(int id)
    {
        return _db.Books
            .Include(b => b.SellerCustomer)
            .ThenInclude(c => c.User)
            .Include(b => b.ReviewedByEmployee)
            .FirstOrDefault(b => b.BookId == id);
    }

    private AppraisalReviewViewModel BuildReviewViewModel(Book book)
    {
        var model = new AppraisalReviewViewModel
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
            ConditionCode = book.ConditionCode,
            ConditionNote = book.ConditionNote,
            ProposedPrice = book.ProposedPrice,
            ApprovedPrice = book.ApprovedPrice,
            ApprovalStatus = book.ApprovalStatus,
            SaleStatus = book.SaleStatus,
            RejectionReason = book.RejectionReason,
            SellerDisplayName = book.SellerCustomer.DisplayName,
            SellerUsername = book.SellerCustomer.User.Username,
            ImageUrl = book.ImageUrl,
            ImageUrl2 = book.ImageUrl2,
            ImageUrl3 = book.ImageUrl3,
            ImageUrl4 = book.ImageUrl4,
            ReviewedByName = book.ReviewedByEmployee?.FullName,
            CreatedAt = book.CreatedAt,
            UpdatedAt = book.UpdatedAt,
            ReviewedAt = book.ReviewedAt
        };

        PopulateOptions(model);
        return model;
    }

    private static void ApplyEditableFields(Book book, AppraisalReviewViewModel model)
    {
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
        book.ConditionCode = model.ConditionCode;
        book.ConditionDiscountPct = ConditionDiscountHelper.ResolvePercent(model.ConditionCode);
        book.ConditionNote = model.ConditionNote;
        book.ProposedPrice = model.ProposedPrice;
        book.ApprovedPrice = model.ApprovedPrice;
    }

    private int? GetCurrentEmployeeId()
    {
        var employeeIdClaim = User.FindFirst("employee_id")?.Value;
        if (int.TryParse(employeeIdClaim, out var employeeId))
        {
            return employeeId;
        }

        return null;
    }

    private static void PopulateOptions(AppraisalReviewViewModel model)
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
}

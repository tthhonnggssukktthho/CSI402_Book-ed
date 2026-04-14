using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _66014444_Project.Models;
using _66014444_Project.ViewModels.Finance;

namespace _66014444_Project.Controllers;

[Authorize(Roles = "admin,finance")]
public class PromotionController : Controller
{
    private readonly _402block2Context _db;

    public PromotionController(_402block2Context db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var now = DateTime.Now;
        var model = new PromotionIndexViewModel
        {
            SystemPromotions =
            [
                new SystemPromotionCardViewModel
                {
                    Title = "ส่วนลดตามสภาพหนังสือ",
                    Description = "ระบบลดให้อัตโนมัติตามสภาพหนังสือที่ผ่านการประเมินแล้ว",
                    Detail = "เหมือนใหม่ ลด 5% | ดี ลด 10% | ตำหนิเล็กน้อย ลด 15% | ตำหนิมาก ลด 20%",
                    AutoApplyNote = "ใช้อัตโนมัติในตะกร้าและตอนชำระเงิน"
                },
                new SystemPromotionCardViewModel
                {
                    Title = "ซื้อครบ 500 บาท ส่งฟรี",
                    Description = "เมื่อยอดสุทธิหลังหักส่วนลดถึง 500 บาท ระบบจะยกเว้นค่าจัดส่งให้อัตโนมัติ",
                    Detail = "ไม่ต้องกรอกโค้ดส่วนลด",
                    AutoApplyNote = "ใช้อัตโนมัติในตะกร้าและตอนชำระเงิน"
                },
                new SystemPromotionCardViewModel
                {
                    Title = "นิยายชุดเดียวกันลดเพิ่ม 50 บาท",
                    Description = "หากในตะกร้ามีหนังสือจากชุดนิยายเดียวกันอย่างน้อย 2 เล่ม ระบบจะลดเพิ่มให้อัตโนมัติ",
                    Detail = "ลดเพิ่ม 50 บาทต่อชุดนิยายที่เข้าเงื่อนไข",
                    AutoApplyNote = "ใช้อัตโนมัติในตะกร้าและตอนชำระเงิน"
                }
            ],
            Promotions = _db.Promotions
                .Include(p => p.PromotionRules)
                .Include(p => p.CreatedByEmployee)
                .Where(p => p.PromotionType == "flash_sale")
                .OrderByDescending(p => p.IsActive == true)
                .ThenByDescending(p => p.StartAt)
                .Select(p => new PromotionSummaryViewModel
                {
                    PromotionId = p.PromotionId,
                    PromotionCode = p.PromotionCode,
                    PromotionName = p.PromotionName,
                    PromotionType = p.PromotionType,
                    DiscountType = p.DiscountType ?? string.Empty,
                    DiscountValue = p.DiscountValue,
                    StartAt = p.StartAt,
                    EndAt = p.EndAt,
                    IsActive = p.IsActive == true,
                    IsAutoApply = p.IsAutoApply,
                    IsExpired = p.EndAt.HasValue && p.EndAt.Value < now,
                    MinOrderAmount = p.MinOrderAmount,
                    MinItemQty = p.MinItemQty,
                    CreatedByEmployeeName = p.CreatedByEmployee != null ? p.CreatedByEmployee.FullName : "-",
                    RuleType = p.PromotionRules.Select(r => r.RuleType).FirstOrDefault() ?? string.Empty,
                    RuleOperator = p.PromotionRules.Select(r => r.RuleOperator).FirstOrDefault() ?? string.Empty,
                    RuleValue = p.PromotionRules.Select(r => r.RuleValue).FirstOrDefault() ?? string.Empty,
                    TargetSeriesName = p.PromotionRules
                        .Where(r => !string.IsNullOrWhiteSpace(r.SeriesName))
                        .Select(r => r.SeriesName!)
                        .FirstOrDefault() ?? "-",
                    TargetBookId = p.PromotionRules.Select(r => r.BookId).FirstOrDefault(),
                    TargetCategoryName = p.PromotionRules
                        .Where(r => !string.IsNullOrWhiteSpace(r.CategoryName))
                        .Select(r => r.CategoryName!)
                        .FirstOrDefault(),
                    RuleCount = p.PromotionRules.Count
                })
                .ToList()
        };

        return View("~/Views/Finance/PromotionIndex.cshtml", model);
    }

    public IActionResult Create()
    {
        return View("~/Views/Finance/PromotionCreate.cshtml", BuildFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(PromotionFormViewModel model)
    {
        PopulateLookups(model);
        model.CreatedByEmployeeId = GetCurrentEmployeeId();
        model.CreatedByEmployeeName = User.Identity?.Name ?? "Staff";

        if (_db.Promotions.Any(p => p.PromotionCode == model.PromotionCode))
        {
            ModelState.AddModelError(nameof(model.PromotionCode), "รหัสโปรโมชันนี้ถูกใช้งานแล้ว");
        }

        ValidatePromotionForm(model);

        if (!ModelState.IsValid)
        {
            return View("~/Views/Finance/PromotionCreate.cshtml", model);
        }

        var now = DateTime.Now;
        var promotion = new Promotion
        {
            PromotionCode = model.PromotionCode.Trim(),
            PromotionName = model.PromotionName.Trim(),
            PromotionType = "flash_sale",
            Description = string.IsNullOrWhiteSpace(model.Description)
                ? $"Flash Sale สำหรับ {model.SelectedSeriesName}"
                : model.Description.Trim(),
            DiscountType = model.DiscountType,
            DiscountValue = model.DiscountValue,
            MinOrderAmount = model.MinOrderAmount,
            MinItemQty = model.MinItemQty,
            StartAt = model.StartAt,
            EndAt = model.EndAt,
            IsActive = model.IsActive,
            IsAutoApply = false,
            CreatedByEmployeeId = model.CreatedByEmployeeId,
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.Promotions.Add(promotion);
        _db.SaveChanges();

        _db.PromotionRules.Add(new PromotionRule
        {
            PromotionId = promotion.PromotionId,
            RuleType = model.RuleType,
            RuleOperator = model.RuleOperator,
            RuleValue = model.RuleValue,
            BookId = model.TargetBookId,
            SeriesName = string.IsNullOrWhiteSpace(model.SelectedSeriesName) ? null : model.SelectedSeriesName.Trim(),
            CategoryName = string.IsNullOrWhiteSpace(model.TargetCategoryName) ? null : model.TargetCategoryName.Trim(),
            CreatedAt = now
        });

        _db.SaveChanges();
        TempData["PromotionSuccess"] = "สร้าง Flash Sale เรียบร้อยแล้ว";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var promotion = _db.Promotions
            .Include(p => p.PromotionRules)
            .Include(p => p.CreatedByEmployee)
            .FirstOrDefault(p => p.PromotionId == id && p.PromotionType == "flash_sale");

        if (promotion is null)
        {
            return NotFound();
        }

        return View("~/Views/Finance/PromotionEdit.cshtml", BuildFormViewModel(promotion));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(PromotionFormViewModel model)
    {
        var promotion = _db.Promotions
            .Include(p => p.PromotionRules)
            .Include(p => p.CreatedByEmployee)
            .FirstOrDefault(p => p.PromotionId == model.PromotionId && p.PromotionType == "flash_sale");

        if (promotion is null)
        {
            return NotFound();
        }

        PopulateLookups(model);
        model.CreatedByEmployeeId = promotion.CreatedByEmployeeId;
        model.CreatedByEmployeeName = promotion.CreatedByEmployee?.FullName ?? User.Identity?.Name ?? "Staff";

        if (_db.Promotions.Any(p => p.PromotionCode == model.PromotionCode && p.PromotionId != model.PromotionId))
        {
            ModelState.AddModelError(nameof(model.PromotionCode), "รหัสโปรโมชันนี้ถูกใช้งานแล้ว");
        }

        ValidatePromotionForm(model);

        if (!ModelState.IsValid)
        {
            return View("~/Views/Finance/PromotionEdit.cshtml", model);
        }

        promotion.PromotionCode = model.PromotionCode.Trim();
        promotion.PromotionName = model.PromotionName.Trim();
        promotion.Description = string.IsNullOrWhiteSpace(model.Description)
            ? $"Flash Sale สำหรับ {model.SelectedSeriesName}"
            : model.Description.Trim();
        promotion.DiscountType = model.DiscountType;
        promotion.DiscountValue = model.DiscountValue;
        promotion.MinOrderAmount = model.MinOrderAmount;
        promotion.MinItemQty = model.MinItemQty;
        promotion.StartAt = model.StartAt;
        promotion.EndAt = model.EndAt;
        promotion.IsActive = model.IsActive;
        promotion.UpdatedAt = DateTime.Now;

        var rule = promotion.PromotionRules.FirstOrDefault();
        if (rule is null)
        {
            promotion.PromotionRules.Add(new PromotionRule
            {
                RuleType = model.RuleType,
                RuleOperator = model.RuleOperator,
                RuleValue = model.RuleValue,
                BookId = model.TargetBookId,
                SeriesName = string.IsNullOrWhiteSpace(model.SelectedSeriesName) ? null : model.SelectedSeriesName.Trim(),
                CategoryName = string.IsNullOrWhiteSpace(model.TargetCategoryName) ? null : model.TargetCategoryName.Trim(),
                CreatedAt = DateTime.Now
            });
        }
        else
        {
            rule.RuleType = model.RuleType;
            rule.RuleOperator = model.RuleOperator;
            rule.RuleValue = model.RuleValue;
            rule.BookId = model.TargetBookId;
            rule.SeriesName = string.IsNullOrWhiteSpace(model.SelectedSeriesName) ? null : model.SelectedSeriesName.Trim();
            rule.CategoryName = string.IsNullOrWhiteSpace(model.TargetCategoryName) ? null : model.TargetCategoryName.Trim();
        }

        _db.SaveChanges();
        TempData["PromotionSuccess"] = "แก้ไข Flash Sale เรียบร้อยแล้ว";
        return RedirectToAction(nameof(Index));
    }

    private PromotionFormViewModel BuildFormViewModel(Promotion? promotion = null)
    {
        var rule = promotion?.PromotionRules.FirstOrDefault();
        var model = new PromotionFormViewModel
        {
            PromotionId = promotion?.PromotionId ?? 0,
            PromotionCode = promotion?.PromotionCode ?? string.Empty,
            PromotionName = promotion?.PromotionName ?? string.Empty,
            Description = promotion?.Description ?? string.Empty,
            DiscountType = promotion?.DiscountType ?? "percent",
            DiscountValue = promotion?.DiscountValue ?? 10m,
            MinOrderAmount = promotion?.MinOrderAmount,
            MinItemQty = promotion?.MinItemQty,
            StartAt = promotion?.StartAt ?? DateTime.Now,
            EndAt = promotion?.EndAt ?? DateTime.Now.AddDays(7),
            IsActive = promotion?.IsActive ?? true,
            RuleType = rule?.RuleType ?? "series_name",
            RuleOperator = rule?.RuleOperator ?? "equals",
            RuleValue = rule?.RuleValue ?? string.Empty,
            SelectedSeriesName = rule?.SeriesName ?? string.Empty,
            TargetBookId = rule?.BookId,
            TargetCategoryName = rule?.CategoryName,
            CreatedByEmployeeId = promotion?.CreatedByEmployeeId,
            CreatedByEmployeeName = promotion?.CreatedByEmployee?.FullName ?? User.Identity?.Name ?? "Staff"
        };

        PopulateLookups(model);
        return model;
    }

    private void PopulateLookups(PromotionFormViewModel model)
    {
        var approvedBooks = _db.Books
            .Where(b => b.ApprovalStatus == "approved")
            .OrderBy(b => b.SeriesName)
            .ThenBy(b => b.Title)
            .ToList();

        model.AvailableSeriesNames = approvedBooks
            .Where(b => !string.IsNullOrWhiteSpace(b.SeriesName))
            .Select(b => b.SeriesName!)
            .Distinct()
            .OrderBy(name => name)
            .ToList();

        model.AvailableCategoryNames = approvedBooks
            .Select(b => b.CategoryName)
            .Distinct()
            .OrderBy(name => name)
            .ToList();

        model.AvailableBooks = approvedBooks
            .Select(b => new PromotionSeriesBookViewModel
            {
                BookId = b.BookId,
                Title = b.Title,
                SeriesName = b.SeriesName ?? string.Empty,
                VolumeNo = b.VolumeNo,
                CategoryName = b.CategoryName,
                ApprovedPrice = b.ApprovedPrice,
                ImageUrl = b.ImageUrl
            })
            .ToList();

        if (string.IsNullOrWhiteSpace(model.RuleType))
        {
            model.RuleType = "series_name";
        }

        if (string.IsNullOrWhiteSpace(model.RuleOperator))
        {
            model.RuleOperator = "equals";
        }

        model.RuleValue = model.RuleType switch
        {
            "series_name" => model.SelectedSeriesName,
            "book_id" => model.TargetBookId?.ToString() ?? string.Empty,
            "category_name" => model.TargetCategoryName ?? string.Empty,
            _ => model.RuleValue
        };

        switch (model.RuleType)
        {
            case "series_name":
                model.TargetBookId = null;
                model.TargetCategoryName = null;
                break;
            case "book_id":
                model.SelectedSeriesName = string.Empty;
                model.TargetCategoryName = null;
                break;
            case "category_name":
                model.SelectedSeriesName = string.Empty;
                model.TargetBookId = null;
                break;
        }
    }

    private void ValidatePromotionForm(PromotionFormViewModel model)
    {
        if (model.EndAt <= model.StartAt)
        {
            ModelState.AddModelError(nameof(model.EndAt), "วันสิ้นสุดต้องมากกว่าวันเริ่มต้น");
        }

        if (model.DiscountValue <= 0)
        {
            ModelState.AddModelError(nameof(model.DiscountValue), "มูลค่าส่วนลดต้องมากกว่า 0");
        }

        if (string.IsNullOrWhiteSpace(model.RuleType) || !new[] { "series_name", "book_id", "category_name" }.Contains(model.RuleType))
        {
            ModelState.AddModelError(nameof(model.RuleType), "กรุณาเลือกประเภทเงื่อนไข");
        }

        if (string.IsNullOrWhiteSpace(model.RuleOperator))
        {
            ModelState.AddModelError(nameof(model.RuleOperator), "กรุณาเลือกตัวดำเนินการเงื่อนไข");
        }

        if (model.RuleType == "series_name" && string.IsNullOrWhiteSpace(model.SelectedSeriesName))
        {
            ModelState.AddModelError(nameof(model.SelectedSeriesName), "กรุณาเลือกชุดนิยายที่ต้องการจัด Flash Sale");
        }

        if (model.RuleType == "book_id" && !model.TargetBookId.HasValue)
        {
            ModelState.AddModelError(nameof(model.TargetBookId), "กรุณาเลือกหนังสือที่ต้องการใช้เป็นเงื่อนไข");
        }

        if (model.RuleType == "category_name" && string.IsNullOrWhiteSpace(model.TargetCategoryName))
        {
            ModelState.AddModelError(nameof(model.TargetCategoryName), "กรุณาเลือกหมวดหมู่หนังสือ");
        }
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
}

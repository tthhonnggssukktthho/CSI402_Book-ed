# ViewModels Structure — Used Novel Store System

ViewModels สำหรับโปรเจกต์ .NET MVC ออกแบบให้ 1 ViewModel ต่อ 1 View
แบ่งตาม Controller เพื่อให้หาง่ายและตรงกับโครงสร้าง Views

---

## โครงสร้างโฟลเดอร์

```text
ViewModels/
│
├── Account/
│   ├── RegisterViewModel.cs
│   └── LoginViewModel.cs
│
├── Customer/
│   ├── ProfileViewModel.cs
│   └── MyBooksViewModel.cs
│
├── BookCatalog/
│   ├── BookCatalogIndexViewModel.cs
│   └── BookDetailViewModel.cs
│
├── SellBook/
│   ├── SellBookCreateViewModel.cs
│   └── SellBookEditViewModel.cs
│
├── Cart/
│   └── CartIndexViewModel.cs
│
├── Checkout/
│   └── CheckoutViewModel.cs
│
├── Order/
│   ├── OrderIndexViewModel.cs
│   └── OrderDetailViewModel.cs
│
├── Payment/
│   └── PaymentUploadViewModel.cs
│
├── Points/
│   └── PointsHistoryViewModel.cs
│
├── Appraisal/
│   ├── AppraisalQueueViewModel.cs
│   └── AppraisalReviewViewModel.cs
│
├── Finance/
│   ├── FinanceQueueViewModel.cs
│   ├── FinancePaymentDetailViewModel.cs
│   ├── PromotionIndexViewModel.cs
│   └── PromotionFormViewModel.cs
│
├── Shipping/
│   ├── ShippingQueueViewModel.cs
│   ├── ShippingPackDetailViewModel.cs
│   └── ShippingUpdateTrackingViewModel.cs
│
└── Admin/
    ├── AdminDashboardViewModel.cs
    ├── AdminCustomerListViewModel.cs
    ├── AdminCustomerDetailViewModel.cs
    ├── AdminEmployeeListViewModel.cs
    └── AdminEmployeeFormViewModel.cs
```

---

## 1) Account

### `RegisterViewModel.cs`
```csharp
public class RegisterViewModel
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "รหัสผ่านไม่ตรงกัน")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    [Required]
    [StringLength(100)]
    public string DisplayName { get; set; }

    [Phone]
    [StringLength(20)]
    public string PhoneNumber { get; set; }
}
```

---

### `LoginViewModel.cs`
```csharp
public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}
```

---

## 2) Customer

### `ProfileViewModel.cs`
```csharp
public class ProfileViewModel
{
    // ข้อมูลส่วนตัว
    [Required]
    [StringLength(100)]
    public string DisplayName { get; set; }

    [Phone]
    [StringLength(20)]
    public string PhoneNumber { get; set; }

    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    // ที่อยู่จัดส่ง
    [StringLength(100)]
    public string ReceiverName { get; set; }

    [Phone]
    [StringLength(20)]
    public string ReceiverPhone { get; set; }

    [StringLength(255)]
    public string AddressLine1 { get; set; }

    [StringLength(255)]
    public string AddressLine2 { get; set; }

    [StringLength(100)]
    public string Subdistrict { get; set; }

    [StringLength(100)]
    public string District { get; set; }

    [StringLength(100)]
    public string Province { get; set; }

    [StringLength(10)]
    public string PostalCode { get; set; }

    // แต้มสะสม (read-only)
    public int CurrentPoints { get; set; }
}
```

---

### `MyBooksViewModel.cs`
```csharp
public class MyBooksViewModel
{
    public List<MyBookItemViewModel> Books { get; set; } = new();

    // filter
    public string FilterApprovalStatus { get; set; }  // all | pending | under_review | approved | rejected
    public string FilterSaleStatus { get; set; }       // all | ready_for_sale | sold | removed
}

public class MyBookItemViewModel
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string SeriesName { get; set; }
    public string VolumeNo { get; set; }
    public string ConditionCode { get; set; }
    public decimal? ProposedPrice { get; set; }
    public decimal? ApprovedPrice { get; set; }
    public string ApprovalStatus { get; set; }
    public string SaleStatus { get; set; }
    public string RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool CanEdit { get; set; }  // true เมื่อ approval_status = pending | rejected
}
```

---

## 3) BookCatalog

### `BookCatalogIndexViewModel.cs`
```csharp
public class BookCatalogIndexViewModel
{
    public List<BookCardViewModel> Books { get; set; } = new();

    // search & filter
    public string SearchKeyword { get; set; }
    public string FilterCategory { get; set; }
    public string FilterCondition { get; set; }  // LIKE_NEW | GOOD | MINOR_DEFECT | MAJOR_DEFECT
    public decimal? PriceMin { get; set; }
    public decimal? PriceMax { get; set; }

    // pagination
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public int PageSize { get; set; } = 12;

    // dropdown options
    public List<string> Categories { get; set; } = new();
}

public class BookCardViewModel
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string SeriesName { get; set; }
    public string VolumeNo { get; set; }
    public string AuthorName { get; set; }
    public string ImageUrl { get; set; }
    public string ConditionCode { get; set; }
    public decimal ConditionDiscountPct { get; set; }
    public decimal ApprovedPrice { get; set; }
    public decimal FinalPrice { get; set; }  // ApprovedPrice * (1 - ConditionDiscountPct/100)
}
```

---

### `BookDetailViewModel.cs`
```csharp
public class BookDetailViewModel
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string SeriesName { get; set; }
    public string VolumeNo { get; set; }
    public string CategoryName { get; set; }
    public string PublisherName { get; set; }
    public string AuthorName { get; set; }
    public string Isbn { get; set; }
    public int? PublishYear { get; set; }
    public string Synopsis { get; set; }
    public string BookDescription { get; set; }
    public string ImageUrl { get; set; }

    // สภาพ
    public string ConditionCode { get; set; }
    public decimal ConditionDiscountPct { get; set; }
    public string ConditionNote { get; set; }

    // ราคา
    public decimal ApprovedPrice { get; set; }
    public decimal FinalPrice { get; set; }
    public decimal DiscountAmount { get; set; }

    // สถานะ
    public string SaleStatus { get; set; }
    public bool CanAddToCart { get; set; }  // true เมื่อ sale_status = ready_for_sale และ login แล้ว
    public bool IsAlreadyInCart { get; set; }
}
```

---

## 4) SellBook

### `SellBookCreateViewModel.cs`
```csharp
public class SellBookCreateViewModel
{
    [Required(ErrorMessage = "กรุณากรอกชื่อหนังสือ")]
    [StringLength(255)]
    public string Title { get; set; }

    [StringLength(255)]
    public string SeriesName { get; set; }

    [StringLength(20)]
    public string VolumeNo { get; set; }

    [Required(ErrorMessage = "กรุณาเลือกหมวดหมู่")]
    [StringLength(100)]
    public string CategoryName { get; set; }

    [StringLength(150)]
    public string AuthorName { get; set; }

    [StringLength(150)]
    public string PublisherName { get; set; }

    [StringLength(20)]
    public string Isbn { get; set; }

    [Range(1800, 2100)]
    public int? PublishYear { get; set; }

    public string Synopsis { get; set; }
    public string BookDescription { get; set; }

    // สภาพและรูปภาพ
    [Required(ErrorMessage = "กรุณาเลือกสภาพหนังสือ")]
    public string ConditionCode { get; set; }  // LIKE_NEW | GOOD | MINOR_DEFECT | MAJOR_DEFECT

    [StringLength(255)]
    public string ConditionNote { get; set; }

    public IFormFile ImageFile { get; set; }  // รูปอัปโหลด

    [Range(0, 99999)]
    public decimal? ProposedPrice { get; set; }

    // dropdown options
    public List<string> Categories { get; set; } = new();
    public List<SelectListItem> ConditionOptions { get; set; } = new();
}
```

---

### `SellBookEditViewModel.cs`
```csharp
public class SellBookEditViewModel : SellBookCreateViewModel
{
    public int BookId { get; set; }
    public string ExistingImageUrl { get; set; }  // รูปเดิม (ถ้าไม่อัปโหลดใหม่)
    public string ApprovalStatus { get; set; }    // read-only แสดงเหตุผลถ้า rejected
    public string RejectionReason { get; set; }
}
```

---

## 5) Cart

### `CartIndexViewModel.cs`
```csharp
public class CartIndexViewModel
{
    public List<CartItemViewModel> Items { get; set; } = new();
    public decimal Subtotal { get; set; }
    public bool IsEmpty => !Items.Any();
}

public class CartItemViewModel
{
    public int CartItemId { get; set; }
    public int BookId { get; set; }
    public string Title { get; set; }
    public string SeriesName { get; set; }
    public string VolumeNo { get; set; }
    public string ImageUrl { get; set; }
    public string ConditionCode { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ConditionDiscountPct { get; set; }
    public decimal FinalPrice { get; set; }
    public string SaleStatus { get; set; }        // เช็กว่ายังพร้อมขายอยู่ไหม
    public bool IsAvailable { get; set; }         // false = หนังสือถูกซื้อไปแล้ว
}
```

---

## 6) Checkout

### `CheckoutViewModel.cs`
```csharp
public class CheckoutViewModel
{
    // ที่อยู่จัดส่ง (จาก customers)
    public string ReceiverName { get; set; }
    public string ReceiverPhone { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string Subdistrict { get; set; }
    public string District { get; set; }
    public string Province { get; set; }
    public string PostalCode { get; set; }

    // รายการสินค้า
    public List<CheckoutItemViewModel> Items { get; set; } = new();

    // โปรโมชัน
    public string PromotionCode { get; set; }         // input กรอก
    public int? AppliedPromotionId { get; set; }
    public string AppliedPromotionName { get; set; }
    public string PromotionErrorMessage { get; set; }

    // แต้ม
    public int AvailablePoints { get; set; }
    public bool UsePoints { get; set; }
    public int PointsToUse { get; set; }
    public decimal PointsDiscountAmount { get; set; }  // PointsToUse * POINT_REDEEM_VALUE

    // ยอดเงิน (คำนวณฝั่ง server)
    public decimal SubtotalAmount { get; set; }
    public decimal ConditionDiscountAmount { get; set; }
    public decimal PromotionDiscountAmount { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal TotalAmount { get; set; }
    public int PointsToEarn { get; set; }              // แต้มที่จะได้รับหลังชำระ
}

public class CheckoutItemViewModel
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string SeriesName { get; set; }
    public string ConditionCode { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ConditionDiscountPct { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal NetAmount { get; set; }
}
```

---

## 7) Order

### `OrderIndexViewModel.cs`
```csharp
public class OrderIndexViewModel
{
    public List<OrderSummaryViewModel> Orders { get; set; } = new();

    // filter
    public string FilterStatus { get; set; }  // all | pending_payment | payment_submitted | ... | completed

    // pagination
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 10;
}

public class OrderSummaryViewModel
{
    public int OrderId { get; set; }
    public string OrderNo { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ItemCount { get; set; }
    public decimal TotalAmount { get; set; }
    public string OrderStatus { get; set; }
    public DateTime PaymentDueAt { get; set; }
    public bool IsExpiringSoon { get; set; }  // true เมื่อ pending_payment และเหลือ < 1 ชม.
}
```

---

### `OrderDetailViewModel.cs`
```csharp
public class OrderDetailViewModel
{
    // คำสั่งซื้อ
    public int OrderId { get; set; }
    public string OrderNo { get; set; }
    public string OrderStatus { get; set; }
    public string PreviousStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime PaymentDueAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string CancelReason { get; set; }

    // ที่อยู่ snapshot
    public string SnapReceiverName { get; set; }
    public string SnapReceiverPhone { get; set; }
    public string SnapAddressLine1 { get; set; }
    public string SnapAddressLine2 { get; set; }
    public string SnapSubdistrict { get; set; }
    public string SnapDistrict { get; set; }
    public string SnapProvince { get; set; }
    public string SnapPostalCode { get; set; }

    // รายการสินค้า
    public List<OrderItemViewModel> Items { get; set; } = new();

    // ยอดเงิน
    public decimal SubtotalAmount { get; set; }
    public decimal ConditionDiscountAmount { get; set; }
    public decimal PromotionDiscountAmount { get; set; }
    public decimal PointsDiscountAmount { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal TotalAmount { get; set; }
    public int PointsEarned { get; set; }
    public int PointsUsed { get; set; }

    // การชำระเงิน
    public string PaymentStatus { get; set; }
    public decimal? TransferAmount { get; set; }
    public DateTime? TransferDatetime { get; set; }
    public string PayerName { get; set; }
    public string EvidenceUrl { get; set; }
    public string RejectReason { get; set; }
    public bool CanUploadPayment { get; set; }  // true เมื่อ order_status = pending_payment

    // การจัดส่ง
    public string ShipmentStatus { get; set; }
    public string CarrierName { get; set; }
    public string TrackingNo { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
}

public class OrderItemViewModel
{
    public int OrderItemId { get; set; }
    public string BookTitleSnapshot { get; set; }
    public string SeriesNameSnapshot { get; set; }
    public string ConditionCodeSnap { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal NetAmount { get; set; }
}
```

---

## 8) Payment

### `PaymentUploadViewModel.cs`
```csharp
public class PaymentUploadViewModel
{
    public int OrderId { get; set; }
    public string OrderNo { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime PaymentDueAt { get; set; }

    // form fields
    [Required(ErrorMessage = "กรุณากรอกจำนวนเงินที่โอน")]
    [Range(0.01, double.MaxValue)]
    public decimal TransferAmount { get; set; }

    [Required(ErrorMessage = "กรุณากรอกวันเวลาที่โอน")]
    public DateTime TransferDatetime { get; set; }

    [StringLength(100)]
    public string PayerName { get; set; }

    [Required(ErrorMessage = "กรุณาแนบหลักฐานการโอนเงิน")]
    public IFormFile EvidenceFile { get; set; }
}
```

---

## 9) Points

### `PointsHistoryViewModel.cs`
```csharp
public class PointsHistoryViewModel
{
    public int CurrentPoints { get; set; }
    public List<PointTransactionViewModel> Transactions { get; set; } = new();

    // filter
    public string FilterType { get; set; }    // all | earn | redeem | adjust
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }

    // pagination
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 20;
}

public class PointTransactionViewModel
{
    public int PointTxnId { get; set; }
    public string TransactionType { get; set; }  // earn | redeem | expire | adjust
    public int Points { get; set; }              // บวก = ได้รับ, ลบ = ใช้ไป
    public string Description { get; set; }
    public string OrderNo { get; set; }          // null ถ้าไม่เกี่ยวกับ order
    public DateTime CreatedAt { get; set; }
}
```

---

## 10) Appraisal

### `AppraisalQueueViewModel.cs`
```csharp
public class AppraisalQueueViewModel
{
    public List<AppraisalQueueItemViewModel> Books { get; set; } = new();

    // filter
    public string FilterStatus { get; set; }  // all | pending | under_review

    // pagination
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 20;
}

public class AppraisalQueueItemViewModel
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string SeriesName { get; set; }
    public string SellerDisplayName { get; set; }
    public string ConditionCode { get; set; }
    public decimal? ProposedPrice { get; set; }
    public string ApprovalStatus { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

---

### `AppraisalReviewViewModel.cs`
```csharp
public class AppraisalReviewViewModel
{
    // ข้อมูลหนังสือ (read-only แสดงบนหน้า)
    public int BookId { get; set; }
    public string Title { get; set; }
    public string SeriesName { get; set; }
    public string VolumeNo { get; set; }
    public string CategoryName { get; set; }
    public string AuthorName { get; set; }
    public string PublisherName { get; set; }
    public string Synopsis { get; set; }
    public string ImageUrl { get; set; }
    public string ConditionCode { get; set; }
    public string ConditionNote { get; set; }
    public decimal? ProposedPrice { get; set; }
    public string SellerDisplayName { get; set; }
    public DateTime CreatedAt { get; set; }

    // form fields
    [Required(ErrorMessage = "กรุณากรอกราคาขาย")]
    [Range(0.01, double.MaxValue)]
    public decimal ApprovedPrice { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal ConditionDiscountPct { get; set; }

    [StringLength(255)]
    public string ReviewNote { get; set; }

    [StringLength(255)]
    public string RejectionReason { get; set; }  // required เมื่อ action = reject

    public string Action { get; set; }  // "approve" | "reject"
}
```

---

## 11) Finance

### `FinanceQueueViewModel.cs`
```csharp
public class FinanceQueueViewModel
{
    public List<FinanceQueueItemViewModel> Payments { get; set; } = new();

    // filter
    public string FilterStatus { get; set; }  // all | pending | submitted

    // pagination
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 20;
}

public class FinanceQueueItemViewModel
{
    public int PaymentId { get; set; }
    public string OrderNo { get; set; }
    public string CustomerDisplayName { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TransferAmount { get; set; }
    public string PaymentStatus { get; set; }
    public DateTime? EvidenceUploadedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

---

### `FinancePaymentDetailViewModel.cs`
```csharp
public class FinancePaymentDetailViewModel
{
    // คำสั่งซื้อ
    public int OrderId { get; set; }
    public string OrderNo { get; set; }
    public string CustomerDisplayName { get; set; }
    public List<OrderItemViewModel> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }

    // การชำระเงิน
    public int PaymentId { get; set; }
    public string PaymentStatus { get; set; }
    public decimal TransferAmount { get; set; }
    public DateTime? TransferDatetime { get; set; }
    public string PayerName { get; set; }
    public string EvidenceUrl { get; set; }
    public DateTime? EvidenceUploadedAt { get; set; }

    // form fields
    [StringLength(255)]
    public string RejectReason { get; set; }

    public string Action { get; set; }  // "approve" | "reject"
}
```

---

### `PromotionIndexViewModel.cs`
```csharp
public class PromotionIndexViewModel
{
    public List<PromotionSummaryViewModel> Promotions { get; set; } = new();

    // filter
    public string FilterType { get; set; }
    public bool? FilterActive { get; set; }

    // pagination
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 20;
}

public class PromotionSummaryViewModel
{
    public int PromotionId { get; set; }
    public string PromotionCode { get; set; }
    public string PromotionName { get; set; }
    public string PromotionType { get; set; }
    public string DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsAutoApply { get; set; }
    public bool IsExpired { get; set; }  // EndAt < DateTime.Now
}
```

---

### `PromotionFormViewModel.cs`
```csharp
// ใช้ร่วมกันทั้ง Create และ Edit
public class PromotionFormViewModel
{
    public int? PromotionId { get; set; }  // null = Create, มีค่า = Edit

    [Required]
    [StringLength(50)]
    public string PromotionCode { get; set; }

    [Required]
    [StringLength(150)]
    public string PromotionName { get; set; }

    [Required]
    public string PromotionType { get; set; }
        // condition_discount | free_shipping | series_discount | flash_sale | points_rule

    [Required]
    [StringLength(500)]
    public string Description { get; set; }

    public string DiscountType { get; set; }   // percent | amount | free_shipping
    public decimal? DiscountValue { get; set; }
    public decimal? MinOrderAmount { get; set; }
    public int? MinItemQty { get; set; }

    [Required]
    public DateTime StartAt { get; set; }

    public DateTime? EndAt { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsAutoApply { get; set; }

    // กฎโปรโมชัน (dynamic list)
    public List<PromotionRuleFormViewModel> Rules { get; set; } = new();

    // dropdown options
    public List<SelectListItem> PromotionTypeOptions { get; set; } = new();
    public List<SelectListItem> DiscountTypeOptions { get; set; } = new();
}

public class PromotionRuleFormViewModel
{
    public int? RuleId { get; set; }  // null = ใหม่

    [Required]
    public string RuleType { get; set; }  // condition | book_scope | series_scope | category_scope

    public string RuleOperator { get; set; }  // = | >= | IN
    public string RuleValue { get; set; }
    public int? BookId { get; set; }

    [StringLength(255)]
    public string SeriesName { get; set; }

    [StringLength(100)]
    public string CategoryName { get; set; }

    public bool IsDeleted { get; set; }  // mark เพื่อลบตอน save
}
```

---

## 12) Shipping

### `ShippingQueueViewModel.cs`
```csharp
public class ShippingQueueViewModel
{
    public List<ShippingQueueItemViewModel> Shipments { get; set; } = new();

    // filter
    public string FilterStatus { get; set; }  // all | pending | ready_to_pack | packed

    // pagination
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 20;
}

public class ShippingQueueItemViewModel
{
    public int ShipmentId { get; set; }
    public string OrderNo { get; set; }
    public string CustomerDisplayName { get; set; }
    public string SnapProvince { get; set; }
    public int ItemCount { get; set; }
    public string ShipmentStatus { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

---

### `ShippingPackDetailViewModel.cs`
```csharp
public class ShippingPackDetailViewModel
{
    public int ShipmentId { get; set; }
    public string OrderNo { get; set; }
    public string ShipmentStatus { get; set; }

    // ที่อยู่จัดส่ง (สำหรับพิมพ์ใบจ่าหน้า)
    public string SnapReceiverName { get; set; }
    public string SnapReceiverPhone { get; set; }
    public string SnapAddressLine1 { get; set; }
    public string SnapAddressLine2 { get; set; }
    public string SnapSubdistrict { get; set; }
    public string SnapDistrict { get; set; }
    public string SnapProvince { get; set; }
    public string SnapPostalCode { get; set; }

    // รายการสินค้า
    public List<OrderItemViewModel> Items { get; set; } = new();
}
```

---

### `ShippingUpdateTrackingViewModel.cs`
```csharp
public class ShippingUpdateTrackingViewModel
{
    public int ShipmentId { get; set; }
    public string OrderNo { get; set; }
    public string CurrentStatus { get; set; }

    [Required(ErrorMessage = "กรุณากรอกชื่อบริษัทขนส่ง")]
    [StringLength(100)]
    public string CarrierName { get; set; }

    [Required(ErrorMessage = "กรุณากรอกเลขติดตามพัสดุ")]
    [StringLength(100)]
    public string TrackingNo { get; set; }

    [Required]
    public string ShipmentStatus { get; set; }  // packed | shipped | delivered

    // dropdown options
    public List<SelectListItem> CarrierOptions { get; set; } = new();
    public List<SelectListItem> StatusOptions { get; set; } = new();
}
```

---

## 13) Admin

### `AdminDashboardViewModel.cs`
```csharp
public class AdminDashboardViewModel
{
    // card สรุป
    public int PendingAppraisalCount { get; set; }   // books ที่ approval_status = pending
    public int PendingPaymentCount { get; set; }      // payments ที่ payment_status = submitted
    public int PendingShipmentCount { get; set; }     // shipments ที่ shipment_status = pending/ready_to_pack
    public int TotalCustomers { get; set; }
    public int TotalEmployees { get; set; }
    public int TotalBooksForSale { get; set; }        // sale_status = ready_for_sale
}
```

---

### `AdminCustomerListViewModel.cs`
```csharp
public class AdminCustomerListViewModel
{
    public List<AdminCustomerItemViewModel> Customers { get; set; } = new();

    // search & filter
    public string SearchKeyword { get; set; }  // ชื่อ | อีเมล | เบอร์
    public string FilterStatus { get; set; }   // all | active | suspended

    // pagination
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 20;
}

public class AdminCustomerItemViewModel
{
    public int CustomerId { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Status { get; set; }
    public int CurrentPoints { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

---

### `AdminCustomerDetailViewModel.cs`
```csharp
public class AdminCustomerDetailViewModel
{
    // ข้อมูลลูกค้า
    public int CustomerId { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public string Status { get; set; }
    public int CurrentPoints { get; set; }
    public DateTime CreatedAt { get; set; }

    // ที่อยู่
    public string ReceiverName { get; set; }
    public string AddressLine1 { get; set; }
    public string Province { get; set; }
    public string PostalCode { get; set; }

    // form สำหรับปรับแต้ม
    public int AdjustPoints { get; set; }
    [StringLength(255)]
    public string AdjustReason { get; set; }
}
```

---

### `AdminEmployeeListViewModel.cs`
```csharp
public class AdminEmployeeListViewModel
{
    public List<AdminEmployeeItemViewModel> Employees { get; set; } = new();

    // filter
    public string FilterStatus { get; set; }  // all | active | inactive | resigned

    // pagination
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 20;
}

public class AdminEmployeeItemViewModel
{
    public int EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }  // Appraisal | Finance | Shipping | Admin
    public string EmploymentStatus { get; set; }
    public DateTime HireDate { get; set; }
}
```

---

### `AdminEmployeeFormViewModel.cs`
```csharp
// ใช้ร่วมกันทั้ง Create และ Edit
public class AdminEmployeeFormViewModel
{
    public int? EmployeeId { get; set; }  // null = Create, มีค่า = Edit

    // account (เฉพาะ Create)
    [StringLength(50)]
    public string Username { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; }

    // ข้อมูลพนักงาน
    [Required]
    [StringLength(150)]
    public string FullName { get; set; }

    [Required]
    [StringLength(30)]
    public string EmployeeCode { get; set; }

    [Required]
    public string Role { get; set; }  // Appraisal | Finance | Shipping | Admin

    [Required]
    public DateTime HireDate { get; set; }

    public string EmploymentStatus { get; set; }  // active | inactive | resigned (เฉพาะ Edit)
    public DateTime? ResignDate { get; set; }

    // dropdown options
    public List<SelectListItem> RoleOptions { get; set; } = new();
    public List<SelectListItem> StatusOptions { get; set; } = new();
}
```

---

## สรุป ViewModels ทั้งหมด

| Controller | ViewModels | จำนวน |
|---|---|---|
| Account | RegisterViewModel, LoginViewModel | 2 |
| Customer | ProfileViewModel, MyBooksViewModel + MyBookItemViewModel | 3 |
| BookCatalog | BookCatalogIndexViewModel + BookCardViewModel, BookDetailViewModel | 3 |
| SellBook | SellBookCreateViewModel, SellBookEditViewModel | 2 |
| Cart | CartIndexViewModel + CartItemViewModel | 2 |
| Checkout | CheckoutViewModel + CheckoutItemViewModel | 2 |
| Order | OrderIndexViewModel + OrderSummaryViewModel, OrderDetailViewModel + OrderItemViewModel | 4 |
| Payment | PaymentUploadViewModel | 1 |
| Points | PointsHistoryViewModel + PointTransactionViewModel | 2 |
| Appraisal | AppraisalQueueViewModel + AppraisalQueueItemViewModel, AppraisalReviewViewModel | 3 |
| Finance | FinanceQueueViewModel + FinanceQueueItemViewModel, FinancePaymentDetailViewModel, PromotionIndexViewModel + PromotionSummaryViewModel, PromotionFormViewModel + PromotionRuleFormViewModel | 7 |
| Shipping | ShippingQueueViewModel + ShippingQueueItemViewModel, ShippingPackDetailViewModel, ShippingUpdateTrackingViewModel | 4 |
| Admin | AdminDashboardViewModel, AdminCustomerListViewModel + AdminCustomerItemViewModel, AdminCustomerDetailViewModel, AdminEmployeeListViewModel + AdminEmployeeItemViewModel, AdminEmployeeFormViewModel | 7 |
| **รวม** | | **42 classes** |

> `OrderItemViewModel` ใช้ร่วมกันใน Order, Finance, และ Shipping — แนะนำให้วางไว้ใน `ViewModels/Shared/OrderItemViewModel.cs`

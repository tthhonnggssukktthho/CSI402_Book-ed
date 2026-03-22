# Structure — Used Novel Store System (14-Table Edition)

เอกสารนี้สรุป **โครงสร้างระบบ + โครงสร้างฐานข้อมูล** สำหรับโปรเจกต์ร้านขายนิยายมือสอง
ปรับให้เหมาะกับโปรเจกต์ .NET MVC โดยลดจาก 25 → **14 ตาราง** โดยยังรองรับทุก feature ในโจทย์

---

## 1) บทบาทผู้ใช้งาน (Roles)

### 1.1 Customer
- สมัครสมาชิก / เข้าสู่ระบบ
- จัดการโปรไฟล์และที่อยู่จัดส่ง
- ลงหนังสือที่ต้องการขาย / ดูสถานะการตรวจสอบ
- เพิ่มสินค้าในตะกร้า / สร้างคำสั่งซื้อ
- ใช้โปรโมชัน / ใช้แต้มสะสม
- ชำระเงินและแนบหลักฐาน / ติดตามคำสั่งซื้อ

### 1.2 Appraisal Staff (พนักงานตรวจสอบ)
- ดูและตรวจสอบหนังสือที่ลูกค้าส่งเข้ามา
- อนุมัติ / ปฏิเสธ / กำหนดราคาขาย

### 1.3 Finance Staff (พนักงานการเงิน)
- ตรวจสอบและอนุมัติการชำระเงิน
- จัดการโปรโมชัน

### 1.4 Shipping Staff (ฝ่ายจัดส่ง)
- พิมพ์ใบจ่าหน้า / บันทึกเลขติดตาม / อัปเดตสถานะจัดส่ง

### 1.5 Admin
- จัดการบัญชีทั้งลูกค้าและพนักงาน / กำหนดสิทธิ์

> ใช้ **ASP.NET Core Identity + Role-based Authorization** จัดการ Role ทั้งหมด ไม่ต้องสร้างตาราง `roles` / `user_roles` เอง

---

## 2) โครงสร้างระบบแบบ MVC ที่แนะนำ

```text
Controllers/
├── AccountController.cs
├── CustomerController.cs
├── BookCatalogController.cs
├── SellBookController.cs
├── CartController.cs
├── CheckoutController.cs
├── OrderController.cs
├── PaymentController.cs
├── PromotionController.cs
├── PointsController.cs
├── AdminController.cs
├── AppraisalController.cs
├── FinanceController.cs
└── ShippingController.cs

Models/
├── User.cs
├── Customer.cs          -- รวม Address + Points
├── Employee.cs          -- รวม Address
├── Book.cs              -- รวม Condition + Image + ApprovalInfo
├── CartItem.cs
├── Order.cs             -- รวม AddressSnapshot + PreviousStatus
├── OrderItem.cs
├── Payment.cs           -- รวม EvidenceUrl
├── Shipment.cs
├── Promotion.cs
├── PromotionRule.cs
├── PointTransaction.cs
└── SystemSetting.cs
```

---

## 3) ตารางทั้ง 14 และโครงสร้าง

---

## 4) กลุ่มที่ 1 — Users & Auth

### 4.1 `users`
บัญชีกลางของทุกคนในระบบ แมปกับ ASP.NET Core Identity

| ฟิลด์ | ชนิดข้อมูล | รายละเอียด |
|---|---|---|
| user_id (PK) | int | รหัสผู้ใช้ |
| username | nvarchar(50) | ชื่อผู้ใช้ (unique) |
| password_hash | nvarchar(255) | รหัสผ่านแบบเข้ารหัส |
| email | nvarchar(100) | อีเมล (unique) |
| phone_number | nvarchar(20) | เบอร์โทรศัพท์ |
| user_type | nvarchar(20) | `customer` หรือ `employee` |
| is_active | boolean | สภาวการใช้งาน |
| created_at | datetime | วันที่สร้างบัญชี |
| updated_at | datetime | วันที่แก้ไขล่าสุด |
| last_login_at | datetime, null | วันที่เข้าใช้งานล่าสุด |

> Role จัดการผ่าน ASP.NET Core Identity (`AspNetRoles`, `AspNetUserRoles`) ไม่ต้องสร้างตาราง `roles` / `user_roles` เพิ่ม

---

### 4.2 `customers`
ข้อมูลลูกค้า รวมที่อยู่จัดส่ง 1 ที่ และยอดแต้มสะสมปัจจุบัน

| ฟิลด์ | ชนิดข้อมูล | รายละเอียด |
|---|---|---|
| customer_id (PK) | int | รหัสลูกค้า |
| user_id (FK) | int | อ้างถึง `users.user_id` |
| display_name | nvarchar(100) | ชื่อที่แสดง |
| birth_date | date, null | วันเกิด |
| status | nvarchar(20) | `active`, `suspended`, `deleted` |
| receiver_name | nvarchar(100), null | ชื่อผู้รับ |
| receiver_phone | nvarchar(20), null | เบอร์ผู้รับ |
| address_line1 | nvarchar(255), null | บ้านเลขที่ / ถนน |
| address_line2 | nvarchar(255), null | รายละเอียดเพิ่มเติม |
| subdistrict | nvarchar(100), null | แขวง/ตำบล |
| district | nvarchar(100), null | เขต/อำเภอ |
| province | nvarchar(100), null | จังหวัด |
| postal_code | nvarchar(10), null | รหัสไปรษณีย์ |
| current_points | int | แต้มสะสมคงเหลือ (อัปเดตทุกครั้งที่มีธุรกรรมแต้ม) |
| created_at | datetime | วันที่สร้างข้อมูล |

> รวมมาจาก: `customer_addresses` (ที่อยู่เดียว), `point_wallets` (current_points)

---

### 4.3 `employees`
ข้อมูลพนักงาน รวมที่อยู่พนักงาน 1 ที่

| ฟิลด์ | ชนิดข้อมูล | รายละเอียด |
|---|---|---|
| employee_id (PK) | int | รหัสพนักงาน |
| user_id (FK) | int | อ้างถึง `users.user_id` |
| employee_code | nvarchar(30) | รหัสพนักงาน (unique) |
| full_name | nvarchar(150) | ชื่อ-นามสกุล |
| hire_date | date | วันที่เริ่มงาน |
| resign_date | date, null | วันที่ออกจากงาน |
| employment_status | nvarchar(20) | `active`, `inactive`, `resigned` |
| address_line1 | nvarchar(255), null | ที่อยู่หลัก |
| address_line2 | nvarchar(255), null | รายละเอียดเพิ่มเติม |
| subdistrict | nvarchar(100), null | แขวง/ตำบล |
| district | nvarchar(100), null | เขต/อำเภอ |
| province | nvarchar(100), null | จังหวัด |
| postal_code | nvarchar(10), null | รหัสไปรษณีย์ |
| created_by_admin_id (FK) | int, null | แอดมินผู้สร้างบัญชี |
| created_at | datetime | วันที่สร้างข้อมูล |
| updated_at | datetime | วันที่แก้ไขล่าสุด |

> รวมมาจาก: `employee_addresses`

---

## 5) กลุ่มที่ 2 — Books

### 5.1 `books`
ข้อมูลหนังสือที่ลูกค้านำมาลงขาย รวมสภาพ รูปภาพ และข้อมูลการอนุมัติ

| ฟิลด์ | ชนิดข้อมูล | รายละเอียด |
|---|---|---|
| book_id (PK) | int | รหัสหนังสือ |
| title | nvarchar(255) | ชื่อเรื่อง |
| series_name | nvarchar(255), null | ชื่อชุดนิยาย |
| volume_no | nvarchar(20), null | เล่มที่ |
| category_name | nvarchar(100) | หมวดหมู่ |
| publisher_name | nvarchar(150), null | สำนักพิมพ์ |
| author_name | nvarchar(150), null | ผู้แต่ง |
| isbn | nvarchar(20), null | ISBN |
| publish_year | int, null | ปีพิมพ์ |
| synopsis | text, null | เรื่องย่อ |
| book_description | text, null | รายละเอียดเพิ่มเติม |
| image_url | nvarchar(500), null | รูปภาพหลัก (1 รูป) |
| condition_code | nvarchar(30) | `LIKE_NEW`, `GOOD`, `MINOR_DEFECT`, `MAJOR_DEFECT` |
| condition_discount_pct | decimal(5,2) | ส่วนลดตามสภาพหนังสือ (%) |
| condition_note | nvarchar(255), null | คำอธิบายตำหนิ |
| proposed_price | decimal(10,2), null | ราคาที่ลูกค้าคาดหวัง |
| approved_price | decimal(10,2), null | ราคาที่พนักงานตั้ง |
| approval_status | nvarchar(20) | `pending`, `under_review`, `approved`, `rejected` |
| sale_status | nvarchar(20) | `draft`, `waiting_approval`, `ready_for_sale`, `reserved`, `sold`, `removed` |
| reviewed_by_employee_id (FK) | int, null | พนักงานผู้ตรวจสอบ |
| reviewed_at | datetime, null | วันที่ตรวจสอบล่าสุด |
| rejection_reason | nvarchar(255), null | เหตุผลที่ปฏิเสธ |
| seller_customer_id (FK) | int | ลูกค้าที่นำมาลงขาย |
| created_at | datetime | วันที่ลงขาย |
| updated_at | datetime | วันที่แก้ไขล่าสุด |

> รวมมาจาก: `book_conditions` (condition_code, condition_discount_pct), `book_images` (image_url — 1 รูป), `book_approval_logs` (reviewed_by, reviewed_at, rejection_reason — เก็บครั้งล่าสุด)

---

## 6) กลุ่มที่ 3 — Cart & Order

### 6.1 `cart_items`
ตะกร้าสินค้า เก็บระดับ item ตรงๆ ใน DB เพื่อให้คงอยู่หลัง logout

| ฟิลด์ | ชนิดข้อมูล | รายละเอียด |
|---|---|---|
| cart_item_id (PK) | int | รหัสรายการ |
| customer_id (FK) | int | อ้างถึง `customers.customer_id` |
| book_id (FK) | int | อ้างถึง `books.book_id` |
| unit_price | decimal(10,2) | ราคา ณ เวลาที่ใส่ตะกร้า |
| added_at | datetime | วันที่เพิ่มเข้าตะกร้า |

> รวมมาจาก: `carts` (header) ถูกตัดออก ใช้ customer_id แทน — มี unique constraint ที่ `(customer_id, book_id)`

---

### 6.2 `orders`
คำสั่งซื้อหลัก เก็บ snapshot ที่อยู่จัดส่ง ณ เวลาสั่งซื้อ

| ฟิลด์ | ชนิดข้อมูล | รายละเอียด |
|---|---|---|
| order_id (PK) | int | รหัสคำสั่งซื้อ |
| order_no | nvarchar(30) | เลขที่คำสั่งซื้อ (unique) |
| customer_id (FK) | int | อ้างถึง `customers.customer_id` |
| snap_receiver_name | nvarchar(100) | ชื่อผู้รับ ณ เวลาสั่งซื้อ |
| snap_receiver_phone | nvarchar(20) | เบอร์ผู้รับ ณ เวลาสั่งซื้อ |
| snap_address_line1 | nvarchar(255) | ที่อยู่ ณ เวลาสั่งซื้อ |
| snap_address_line2 | nvarchar(255), null | รายละเอียดเพิ่มเติม |
| snap_subdistrict | nvarchar(100) | แขวง/ตำบล |
| snap_district | nvarchar(100) | เขต/อำเภอ |
| snap_province | nvarchar(100) | จังหวัด |
| snap_postal_code | nvarchar(10) | รหัสไปรษณีย์ |
| order_status | nvarchar(30) | สถานะปัจจุบัน |
| previous_status | nvarchar(30), null | สถานะก่อนหน้า |
| subtotal_amount | decimal(10,2) | ยอดรวมก่อนส่วนลด |
| condition_discount_amount | decimal(10,2) | ส่วนลดจากสภาพหนังสือ |
| promotion_discount_amount | decimal(10,2) | ส่วนลดจากโปรโมชัน |
| points_discount_amount | decimal(10,2) | ส่วนลดจากแต้ม |
| shipping_fee | decimal(10,2) | ค่าส่ง |
| total_amount | decimal(10,2) | ยอดสุทธิ |
| points_earned | int | แต้มที่ได้รับจากคำสั่งซื้อนี้ |
| points_used | int | แต้มที่ใช้เป็นส่วนลด |
| promotion_id (FK) | int, null | โปรโมชันที่ใช้ |
| payment_due_at | datetime | กำหนดชำระเงิน (created_at + 24 ชม.) |
| created_at | datetime | วันที่สร้างคำสั่งซื้อ |
| updated_at | datetime | วันที่แก้ไขล่าสุด |
| cancelled_at | datetime, null | วันที่ยกเลิก |
| cancel_reason | nvarchar(255), null | เหตุผลยกเลิก |

> รวมมาจาก: `order_status_histories` → previous_status เก็บในตารางนี้แทน

**สถานะที่เป็นไปได้**
- `pending_payment` → `payment_submitted` → `payment_verified` → `preparing_shipment` → `shipped` → `completed`
- `payment_rejected` / `expired` / `cancelled`

---

### 6.3 `order_items`

| ฟิลด์ | ชนิดข้อมูล | รายละเอียด |
|---|---|---|
| order_item_id (PK) | int | รหัสรายการสินค้า |
| order_id (FK) | int | อ้างถึง `orders.order_id` |
| book_id (FK) | int | อ้างถึง `books.book_id` |
| seller_customer_id (FK) | int | เจ้าของหนังสือ |
| book_title_snapshot | nvarchar(255) | ชื่อหนังสือ ณ เวลาสั่งซื้อ |
| series_name_snapshot | nvarchar(255), null | ชื่อชุด ณ เวลาสั่งซื้อ |
| condition_code_snap | nvarchar(30) | สภาพหนังสือ ณ เวลาสั่งซื้อ |
| unit_price | decimal(10,2) | ราคาต่อชิ้น |
| discount_amount | decimal(10,2) | ส่วนลดของรายการนี้ |
| net_amount | decimal(10,2) | ราคาสุทธิ |
| created_at | datetime | วันที่สร้าง |

---

## 7) กลุ่มที่ 4 — Payment & Shipping

### 7.1 `payments`
ข้อมูลการชำระเงิน รวมไฟล์หลักฐาน

| ฟิลด์ | ชนิดข้อมูล | รายละเอียด |
|---|---|---|
| payment_id (PK) | int | รหัสการชำระเงิน |
| order_id (FK) | int | อ้างถึง `orders.order_id` |
| payment_method | nvarchar(30) | เช่น `bank_transfer` |
| payment_status | nvarchar(30) | `pending`, `submitted`, `approved`, `rejected`, `expired` |
| transfer_amount | decimal(10,2) | จำนวนเงินที่โอน |
| transfer_datetime | datetime, null | วันที่/เวลาโอน |
| payer_name | nvarchar(100), null | ชื่อผู้โอน |
| evidence_url | nvarchar(500), null | ที่อยู่ไฟล์หลักฐาน |
| evidence_uploaded_at | datetime, null | วันที่อัปโหลดหลักฐาน |
| verified_by_employee_id (FK) | int, null | พนักงานผู้ตรวจสอบ |
| verified_at | datetime, null | วันเวลาที่ตรวจสอบ |
| reject_reason | nvarchar(255), null | เหตุผลปฏิเสธ |
| created_at | datetime | วันที่สร้าง |
| updated_at | datetime | วันที่แก้ไข |

> รวมมาจาก: `payment_evidences` (evidence_url, evidence_uploaded_at — 1 หลักฐาน/การชำระ)

---

### 7.2 `shipments`

| ฟิลด์ | ชนิดข้อมูล | รายละเอียด |
|---|---|---|
| shipment_id (PK) | int | รหัสการจัดส่ง |
| order_id (FK) | int | อ้างถึง `orders.order_id` (unique) |
| shipment_status | nvarchar(30) | `pending`, `ready_to_pack`, `packed`, `shipped`, `delivered`, `returned` |
| carrier_name | nvarchar(100), null | ชื่อบริษัทขนส่ง |
| tracking_no | nvarchar(100), null | เลขติดตามพัสดุ |
| shipping_label_printed_at | datetime, null | วันที่พิมพ์ใบจ่าหน้า |
| packed_by_employee_id (FK) | int, null | พนักงานที่แพ็กสินค้า |
| shipped_at | datetime, null | วันที่จัดส่ง |
| delivered_at | datetime, null | วันที่ส่งสำเร็จ |
| created_at | datetime | วันที่สร้าง |
| updated_at | datetime | วันที่แก้ไข |

---

## 8) กลุ่มที่ 5 — Promotions

### 8.1 `promotions`

| ฟิลด์ | ชนิดข้อมูล | รายละเอียด |
|---|---|---|
| promotion_id (PK) | int | รหัสโปรโมชัน |
| promotion_code | nvarchar(50) | โค้ดโปรโมชัน (unique) |
| promotion_name | nvarchar(150) | ชื่อโปรโมชัน |
| promotion_type | nvarchar(30) | `condition_discount`, `free_shipping`, `series_discount`, `flash_sale`, `points_rule` |
| description | nvarchar(500) | รายละเอียด |
| discount_type | nvarchar(20), null | `percent`, `amount`, `free_shipping` |
| discount_value | decimal(10,2), null | มูลค่าส่วนลด |
| min_order_amount | decimal(10,2), null | ยอดขั้นต่ำ |
| min_item_qty | int, null | จำนวนขั้นต่ำ |
| start_at | datetime | วันเริ่ม |
| end_at | datetime, null | วันสิ้นสุด |
| is_active | boolean | เปิดใช้งาน |
| is_auto_apply | boolean | ใช้อัตโนมัติ |
| created_by_employee_id (FK) | int, null | ผู้สร้าง |
| created_at | datetime | วันที่สร้าง |
| updated_at | datetime | วันที่แก้ไข |

---

### 8.2 `promotion_rules`
รวม `promotion_conditions` และ `promotion_book_scopes` เป็นตารางเดียว ใช้ `rule_type` แยกประเภท

| ฟิลด์ | ชนิดข้อมูล | รายละเอียด |
|---|---|---|
| rule_id (PK) | int | รหัสกฎ |
| promotion_id (FK) | int | อ้างถึง `promotions.promotion_id` |
| rule_type | nvarchar(30) | `condition`, `book_scope`, `series_scope`, `category_scope` |
| rule_operator | nvarchar(20), null | `=`, `>=`, `IN` (สำหรับ rule_type = condition) |
| rule_value | nvarchar(255), null | ค่าที่ใช้ตรวจสอบ |
| book_id (FK) | int, null | หนังสือที่ร่วมรายการ (rule_type = book_scope) |
| series_name | nvarchar(255), null | ชุดนิยายที่ร่วมรายการ (rule_type = series_scope) |
| category_name | nvarchar(100), null | หมวดที่ร่วมรายการ (rule_type = category_scope) |
| created_at | datetime | วันที่สร้าง |

> รวมมาจาก: `promotion_conditions` + `promotion_book_scopes`

**ตัวอย่างการเก็บโปรโมชันตามโจทย์**

| โปรโมชัน | promotion_type | rule_type | ตัวอย่าง rule_value |
|---|---|---|---|
| ลดตามสภาพหนังสือ | `condition_discount` | `condition` | condition_code = GOOD |
| ส่งฟรีครบ 500 | `free_shipping` | `condition` | order_amount >= 500 |
| Flash sale ตามชุด | `flash_sale` | `series_scope` | series_name = ชื่อชุด |
| ลดเพิ่มชุดเดียวกัน | `series_discount` | `series_scope` | series_name = ชื่อชุด |

---

## 9) กลุ่มที่ 6 — Points

### 9.1 `point_transactions`
ประวัติการเพิ่ม/ลดแต้มทุกครั้ง ยอดคงเหลือเก็บใน `customers.current_points`

| ฟิลด์ | ชนิดข้อมูล | รายละเอียด |
|---|---|---|
| point_txn_id (PK) | int | รหัสรายการ |
| customer_id (FK) | int | อ้างถึง `customers.customer_id` |
| order_id (FK) | int, null | อ้างถึง `orders.order_id` |
| transaction_type | nvarchar(20) | `earn`, `redeem`, `expire`, `adjust` |
| points | int | บวก = ได้รับ, ลบ = ใช้ไป |
| description | nvarchar(255) | คำอธิบาย |
| created_at | datetime | วันที่ทำรายการ |
| created_by_user_id (FK) | int, null | ผู้ทำรายการ (กรณีแอดมินปรับแต้ม) |

> ตัดออก: `point_wallets` — ยอดคงเหลือเก็บใน `customers.current_points` อัปเดตพร้อมกับทุก transaction

---

## 10) กลุ่มที่ 7 — Settings

### 10.1 `system_settings`

| ฟิลด์ | ชนิดข้อมูล | รายละเอียด |
|---|---|---|
| setting_key (PK) | nvarchar(100) | ชื่อ key |
| setting_value | nvarchar(255) | ค่า |
| description | nvarchar(255), null | คำอธิบาย |
| updated_at | datetime | วันที่แก้ไข |

**ค่าเริ่มต้น**

| Key | Value | ความหมาย |
|---|---|---|
| PAYMENT_EXPIRE_HOURS | 24 | ยกเลิกอัตโนมัติหากไม่ชำระภายใน X ชั่วโมง |
| POINT_EARN_PER_AMOUNT | 100 | ทุก X บาท ได้ 1 แต้ม |
| POINT_EARN_VALUE | 1 | แต้มที่ได้รับต่อรอบ |
| POINT_REDEEM_VALUE | 10 | 1 แต้ม = ลด X บาท |
| FREE_SHIPPING_MIN_AMOUNT | 500 | ยอดขั้นต่ำสำหรับส่งฟรี |

---

## 11) สรุปตารางทั้ง 14

| # | ตาราง | รวมมาจาก / หมายเหตุ |
|---|---|---|
| 1 | `users` | คงเดิม — แมปกับ Identity |
| 2 | `customers` | + address fields + current_points |
| 3 | `employees` | + address fields |
| 4 | `books` | + condition + image_url + approval info |
| 5 | `cart_items` | ตัด `carts` header ออก |
| 6 | `orders` | + address snapshot + previous_status |
| 7 | `order_items` | คงเดิม |
| 8 | `payments` | + evidence_url |
| 9 | `shipments` | คงเดิม |
| 10 | `promotions` | คงเดิม |
| 11 | `promotion_rules` | รวม `promotion_conditions` + `promotion_book_scopes` |
| 12 | `point_transactions` | คงเดิม |
| 13 | `system_settings` | คงเดิม |

**ตารางที่ตัดออกจากเวอร์ชัน 25 ตาราง**

| ตาราง | เหตุผล |
|---|---|
| `roles` / `user_roles` | ใช้ ASP.NET Core Identity แทน |
| `customer_addresses` | รวมเข้า `customers` (1 ที่อยู่) |
| `employee_addresses` | รวมเข้า `employees` |
| `book_conditions` | รวม condition_code / discount_pct เข้า `books` |
| `book_images` | รวม image_url เข้า `books` (1 รูป) |
| `book_approval_logs` | รวม reviewed_by / reviewed_at เข้า `books` |
| `carts` | ตัด header ออก ใช้ customer_id ใน `cart_items` |
| `order_status_histories` | รวม previous_status เข้า `orders` |
| `payment_evidences` | รวม evidence_url เข้า `payments` |
| `promotion_conditions` | รวมเข้า `promotion_rules` |
| `promotion_book_scopes` | รวมเข้า `promotion_rules` |
| `point_wallets` | รวม current_points เข้า `customers` |

---

## 12) ความสัมพันธ์ของตาราง

- `users` 1:1 `customers`
- `users` 1:1 `employees`
- `customers` 1:N `cart_items`
- `customers` 1:N `books` (ในฐานะผู้ลงขาย)
- `customers` 1:N `orders`
- `customers` 1:N `point_transactions`
- `orders` 1:N `order_items`
- `orders` 1:1 `payments`
- `orders` 1:1 `shipments`
- `promotions` 1:N `promotion_rules`
- `promotions` 1:N `orders`

---

## 13) Business Flow หลัก

### ลูกค้านำหนังสือมาลงขาย
1. ลูกค้ากรอกข้อมูลและอัปโหลดรูป → บันทึกลง `books` (`approval_status = pending`)
2. พนักงานตรวจสอบ → อัปเดต `reviewed_by_employee_id`, `reviewed_at`, `approved_price`
3. อนุมัติ → `approval_status = approved`, `sale_status = ready_for_sale`

### ลูกค้าซื้อหนังสือ
1. เพิ่มใน `cart_items`
2. Checkout → สร้าง `orders` + `order_items`, ตั้ง `payment_due_at = now + 24h`
3. อัปโหลดหลักฐาน → อัปเดต `payments.evidence_url`, `payment_status = submitted`
4. Finance อนุมัติ → `payment_status = approved`, `order_status = payment_verified`
5. Shipping แพ็กและส่ง → อัปเดต `shipments.tracking_no`, `shipment_status = shipped`

### ระบบแต้มสะสม
1. ชำระสำเร็จ → คำนวณแต้มจาก `POINT_EARN_PER_AMOUNT`
2. บันทึก `point_transactions` (type = `earn`, points = +N)
3. อัปเดต `customers.current_points += N`
4. ใช้แต้ม → บันทึก `point_transactions` (type = `redeem`, points = -N), อัปเดต `current_points`

### ยกเลิกอัตโนมัติ (Background Job)
1. ตรวจ `orders` ที่ `order_status = pending_payment` และ `payment_due_at < NOW()`
2. อัปเดต `previous_status = pending_payment`, `order_status = expired`
3. คืน `books.sale_status = ready_for_sale`

---

## 14) ข้อแนะนำสำหรับการขยายในอนาคต

หากต้องการเพิ่ม feature ภายหลัง สามารถทำได้โดยไม่กระทบโครงสร้างเดิม

| Feature | วิธีขยาย |
|---|---|
| ลูกค้ามีหลายที่อยู่ | เพิ่มตาราง `customer_addresses` กลับมา + FK จาก `orders` |
| หนังสือมีหลายรูป | เพิ่มตาราง `book_images` กลับมา + FK จาก `books` |
| ประวัติสถานะคำสั่งซื้อ | เพิ่มตาราง `order_status_histories` กลับมา |
| หลักฐานชำระหลายรูป | เพิ่มตาราง `payment_evidences` กลับมา |

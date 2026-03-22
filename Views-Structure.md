# Views Structure — Used Novel Store System

โครงสร้าง Views สำหรับโปรเจกต์ .NET MVC แบ่งตาม Controller และ Role ที่เข้าถึงได้

---

## โครงสร้างโฟลเดอร์

```text
Views/
│
├── Shared/                            -- Layout และ Partial ที่ใช้ร่วมกัน
│   ├── _Layout.cshtml                 -- Layout หลัก (Navbar + Footer)
│   ├── _LayoutStaff.cshtml            -- Layout พนักงาน (Staff Sidebar)
│   ├── _Navbar.cshtml                 -- Navbar แยก partial
│   ├── _BookCard.cshtml               -- Card หนังสือ (ใช้ใน Catalog + Search)
│   ├── _OrderStatusBadge.cshtml       -- Badge สถานะคำสั่งซื้อ
│   ├── _Pagination.cshtml             -- Pagination component
│   └── Error.cshtml                   -- หน้า Error กลาง
│
├── Account/                           -- [AllowAnonymous]
│   ├── Register.cshtml                -- สมัครสมาชิก
│   ├── Login.cshtml                   -- เข้าสู่ระบบ
│   └── AccessDenied.cshtml            -- ไม่มีสิทธิ์เข้าถึง
│
├── Customer/                          -- [Authorize(Roles = "Customer")]
│   ├── Profile.cshtml                 -- ดู/แก้ไขโปรไฟล์ + ที่อยู่ + แต้ม
│   └── MyBooks.cshtml                 -- หนังสือที่ฉันลงขาย + สถานะการตรวจสอบ
│
├── BookCatalog/                       -- [AllowAnonymous]
│   ├── Index.cshtml                   -- รายการหนังสือทั้งหมด + ค้นหา + กรอง
│   └── Detail.cshtml                  -- รายละเอียดหนังสือ + ปุ่มเพิ่มตะกร้า
│
├── SellBook/                          -- [Authorize(Roles = "Customer")]
│   ├── Create.cshtml                  -- ฟอร์มลงหนังสือ (ข้อมูล + รูป + สภาพ)
│   └── Edit.cshtml                    -- แก้ไขหนังสือ (ก่อนส่งตรวจสอบ)
│
├── Cart/                              -- [Authorize(Roles = "Customer")]
│   └── Index.cshtml                   -- ตะกร้าสินค้า + ลบรายการ
│
├── Checkout/                          -- [Authorize(Roles = "Customer")]
│   └── Index.cshtml                   -- สรุปคำสั่งซื้อ + โปรโมชัน + แต้ม + ยืนยัน
│
├── Order/                             -- [Authorize(Roles = "Customer")]
│   ├── Index.cshtml                   -- รายการคำสั่งซื้อของฉัน
│   └── Detail.cshtml                  -- รายละเอียดคำสั่งซื้อ + สถานะ + ติดตาม
│
├── Payment/                           -- [Authorize(Roles = "Customer")]
│   └── Upload.cshtml                  -- อัปโหลดหลักฐานการชำระเงิน
│
├── Points/                            -- [Authorize(Roles = "Customer")]
│   └── History.cshtml                 -- ประวัติแต้มสะสม
│
├── Appraisal/                         -- [Authorize(Roles = "Appraisal,Admin")]
│   ├── Queue.cshtml                   -- คิวหนังสือรอตรวจสอบ
│   └── Review.cshtml                  -- ฟอร์มตรวจสอบ + กำหนดราคา + อนุมัติ/ปฏิเสธ
│
├── Finance/                           -- [Authorize(Roles = "Finance,Admin")]
│   ├── Queue.cshtml                   -- คิวการชำระเงินรอตรวจสอบ
│   ├── PaymentDetail.cshtml           -- ดูหลักฐาน + อนุมัติ/ปฏิเสธ
│   ├── PromotionIndex.cshtml          -- รายการโปรโมชัน
│   ├── PromotionCreate.cshtml         -- สร้างโปรโมชัน + กฎ
│   └── PromotionEdit.cshtml           -- แก้ไขโปรโมชัน
│
├── Shipping/                          -- [Authorize(Roles = "Shipping,Admin")]
│   ├── Queue.cshtml                   -- คิวคำสั่งซื้อพร้อมจัดส่ง
│   ├── PackDetail.cshtml              -- รายละเอียดการแพ็ก + พิมพ์ใบจ่าหน้า
│   └── UpdateTracking.cshtml          -- กรอกเลขพัสดุ + อัปเดตสถานะ
│
└── Admin/                             -- [Authorize(Roles = "Admin")]
    ├── Dashboard.cshtml               -- สรุปภาพรวมระบบ
    ├── CustomerList.cshtml            -- รายการลูกค้าทั้งหมด
    ├── CustomerDetail.cshtml          -- ดูข้อมูลลูกค้า + ระงับ/ปลดระงับ
    ├── EmployeeList.cshtml            -- รายการพนักงานทั้งหมด
    ├── EmployeeCreate.cshtml          -- เพิ่มพนักงาน + กำหนด Role
    └── EmployeeEdit.cshtml            -- แก้ไขข้อมูลพนักงาน + เปลี่ยน Role
```

---

## รายละเอียด Views แต่ละหน้า

---

## 1) Shared

### `_Layout.cshtml`
Layout หลักสำหรับลูกค้าและผู้ไม่ได้ล็อกอิน

ส่วนประกอบหลัก:
- Navbar: โลโก้ / ค้นหาหนังสือ / ไอคอนตะกร้า (จำนวน) / เมนู Login หรือ ชื่อผู้ใช้ + Dropdown
- Dropdown เมื่อล็อกอินแล้ว: โปรไฟล์ / หนังสือของฉัน / คำสั่งซื้อ / แต้มของฉัน / ออกจากระบบ
- Footer: ชื่อร้าน

### `_LayoutStaff.cshtml`
Layout สำหรับหน้าพนักงาน มี Sidebar แทน Dropdown

Sidebar แสดงเมนูตาม Role:
- Appraisal: คิวตรวจสอบหนังสือ
- Finance: คิวการชำระเงิน / จัดการโปรโมชัน
- Shipping: คิวจัดส่ง
- Admin: ทุกเมนู + จัดการลูกค้า/พนักงาน

### `_BookCard.cshtml`
Partial card แสดงข้อมูลหนังสือย่อ

ข้อมูลที่แสดง: รูปปก / ชื่อเรื่อง / ชุด + เล่ม / สภาพ (badge) / ราคา / ปุ่มดูรายละเอียด

---

## 2) Account

### `Register.cshtml`
ฟอร์มสมัครสมาชิก

| ฟิลด์ | ประเภท | หมายเหตุ |
|---|---|---|
| username | text | |
| email | email | |
| password | password | |
| confirm_password | password | |
| display_name | text | |
| phone_number | tel | |

Action: `POST /Account/Register`

---

### `Login.cshtml`
ฟอร์มเข้าสู่ระบบ

| ฟิลด์ | ประเภท | หมายเหตุ |
|---|---|---|
| email | email | |
| password | password | |
| remember_me | checkbox | |

Action: `POST /Account/Login` → Redirect ตาม Role

---

## 3) Customer

### `Profile.cshtml`
แสดงและแก้ไขข้อมูลส่วนตัว + ที่อยู่จัดส่ง + แต้มสะสม

ส่วน "ข้อมูลส่วนตัว":

| ฟิลด์ | ประเภท |
|---|---|
| display_name | text |
| phone_number | tel |
| birth_date | date |

ส่วน "ที่อยู่จัดส่ง":

| ฟิลด์ | ประเภท |
|---|---|
| receiver_name | text |
| receiver_phone | tel |
| address_line1 | text |
| address_line2 | text |
| subdistrict | text |
| district | text |
| province | text |
| postal_code | text |

ส่วน "แต้มสะสม": แสดง `current_points` + ลิงก์ไปหน้า `Points/History`

Action: `POST /Customer/UpdateProfile`

---

### `MyBooks.cshtml`
รายการหนังสือที่ลูกค้าลงขายเอง

ตารางแสดง: ชื่อหนังสือ / สภาพ / ราคาที่เสนอ / ราคาที่อนุมัติ / `approval_status` badge / `sale_status` badge / วันที่ลง

กรองตาม: approval_status / sale_status

ปุ่ม: ลงหนังสือใหม่ → `SellBook/Create`

---

## 4) BookCatalog

### `Index.cshtml`
หน้าหลักร้านค้า แสดงหนังสือที่ `sale_status = ready_for_sale`

ส่วนค้นหาและกรอง:
- Search: ชื่อเรื่อง / ผู้แต่ง / ชุดนิยาย / ISBN
- กรอง: หมวดหมู่ / สภาพหนังสือ / ช่วงราคา

แสดงผลโดยใช้ `_BookCard.cshtml` partial แบบ grid

Pagination ด้านล่าง

---

### `Detail.cshtml`
รายละเอียดหนังสือ 1 เล่ม

ข้อมูลที่แสดง:
- รูปปก (image_url)
- ชื่อเรื่อง / ชุดนิยาย / เล่มที่
- ผู้แต่ง / สำนักพิมพ์ / ปีพิมพ์ / ISBN
- หมวดหมู่
- สภาพหนังสือ (condition_code badge) + condition_note
- ราคา + ส่วนลดจากสภาพ (condition_discount_pct)
- เรื่องย่อ / รายละเอียดเพิ่มเติม

ปุ่ม: เพิ่มในตะกร้า (POST /Cart/Add) — แสดงเฉพาะเมื่อล็อกอินแล้วและ sale_status = ready_for_sale

---

## 5) SellBook

### `Create.cshtml`
ฟอร์มลงหนังสือใหม่

ส่วน "ข้อมูลหนังสือ":

| ฟิลด์ | ประเภท | หมายเหตุ |
|---|---|---|
| title | text | required |
| series_name | text | |
| volume_no | text | |
| category_name | select | dropdown หมวดหมู่ |
| author_name | text | |
| publisher_name | text | |
| isbn | text | |
| publish_year | number | |
| synopsis | textarea | |
| book_description | textarea | |

ส่วน "สภาพและรูปภาพ":

| ฟิลด์ | ประเภท | หมายเหตุ |
|---|---|---|
| condition_code | radio / select | LIKE_NEW / GOOD / MINOR_DEFECT / MAJOR_DEFECT |
| condition_note | textarea | คำอธิบายตำหนิ |
| image_url | file upload | รูปปก |
| proposed_price | number | ราคาที่ต้องการ (ไม่บังคับ) |

Action: `POST /SellBook/Create` → Redirect ไป `Customer/MyBooks`

---

### `Edit.cshtml`
แก้ไขข้อมูลหนังสือ (เฉพาะเมื่อ approval_status = pending หรือ rejected)

ฟิลด์เดียวกับ Create แต่มีข้อมูลเดิมกรอกไว้ล่วงหน้า

Action: `POST /SellBook/Edit/{id}`

---

## 6) Cart

### `Index.cshtml`
ตะกร้าสินค้า

ตารางรายการ: รูปปก / ชื่อหนังสือ / สภาพ / ราคา / ปุ่มลบออก

ด้านล่าง: ยอดรวม + ปุ่ม "ดำเนินการชำระเงิน" → `Checkout/Index`

ถ้าตะกร้าว่าง: แสดงข้อความ + ปุ่มไปหน้า BookCatalog

---

## 7) Checkout

### `Index.cshtml`
หน้าสรุปก่อนยืนยันคำสั่งซื้อ (ดำเนินการแบบ 3 ขั้นตอนหรือหน้าเดียวก็ได้)

ส่วน "ที่อยู่จัดส่ง": แสดงข้อมูลจาก `customers` พร้อมปุ่ม "แก้ไข" ลิงก์ไป Profile

ส่วน "รายการสินค้า": ตารางสรุป order_items พร้อมราคาและส่วนลดสภาพ

ส่วน "โปรโมชัน":
- ช่องกรอก promotion_code + ปุ่มใช้โค้ด
- แสดงโปรโมชันที่ auto_apply และตรงเงื่อนไข

ส่วน "แต้มสะสม":
- แสดง current_points ที่มี
- checkbox ใช้แต้ม + จำนวนที่ต้องการใช้
- แสดงส่วนลดที่จะได้รับ

ส่วน "สรุปยอด":

| | |
|---|---|
| ยอดรวม | subtotal_amount |
| ส่วนลดสภาพหนังสือ | - condition_discount_amount |
| ส่วนลดโปรโมชัน | - promotion_discount_amount |
| ส่วนลดแต้ม | - points_discount_amount |
| ค่าจัดส่ง | shipping_fee |
| **ยอดสุทธิ** | **total_amount** |

ปุ่ม: "ยืนยันคำสั่งซื้อ" → `POST /Checkout/Confirm`

---

## 8) Order

### `Index.cshtml`
รายการคำสั่งซื้อทั้งหมดของลูกค้า

ตาราง: order_no / วันที่ / จำนวนรายการ / ยอดสุทธิ / order_status badge / ปุ่มดูรายละเอียด

กรองตาม: order_status

---

### `Detail.cshtml`
รายละเอียดคำสั่งซื้อ

ส่วน "ข้อมูลคำสั่งซื้อ": order_no / วันที่ / order_status badge / previous_status

ส่วน "ที่อยู่จัดส่ง": snap_* fields

ส่วน "รายการสินค้า": ตาราง order_items พร้อม snapshot

ส่วน "การชำระเงิน": payment_status / transfer_amount / transfer_datetime / หลักฐาน (ถ้ามี)
- ปุ่ม "อัปโหลดหลักฐาน" → `Payment/Upload/{order_id}` (เฉพาะ pending_payment)

ส่วน "การจัดส่ง": shipment_status / carrier_name / tracking_no / shipped_at / delivered_at
(แสดงหลังจาก payment_verified)

ส่วน "ยอดเงิน": สรุปทุก discount + total

---

## 9) Payment

### `Upload.cshtml`
อัปโหลดหลักฐานการโอนเงิน

แสดง: order_no / total_amount / payment_due_at (countdown)

ฟอร์ม:

| ฟิลด์ | ประเภท | หมายเหตุ |
|---|---|---|
| transfer_amount | number | จำนวนเงินที่โอน |
| transfer_datetime | datetime-local | วันเวลาที่โอน |
| payer_name | text | ชื่อผู้โอน |
| evidence_file | file | รูปหลักฐาน |

Action: `POST /Payment/Upload/{order_id}`

---

## 10) Points

### `History.cshtml`
ประวัติแต้มสะสม

แสดง: แต้มคงเหลือปัจจุบัน (`current_points`) แบบ highlight

ตาราง `point_transactions`:
- วันที่ / ประเภท (earn/redeem badge) / คำอธิบาย / จำนวน (+/-) / order_no (ถ้ามี)

กรองตาม: transaction_type / ช่วงวันที่

---

## 11) Appraisal (พนักงานตรวจสอบ)

### `Queue.cshtml`
คิวหนังสือรอตรวจสอบ — [Authorize(Roles = "Appraisal,Admin")]

ตาราง: ชื่อหนังสือ / ผู้ลงขาย / สภาพที่ลูกค้าระบุ / ราคาที่เสนอ / วันที่ส่ง / approval_status badge / ปุ่มตรวจสอบ

กรองตาม: approval_status (pending / under_review)

---

### `Review.cshtml`
ฟอร์มตรวจสอบหนังสือ — [Authorize(Roles = "Appraisal,Admin")]

แสดงข้อมูลหนังสือทั้งหมดรวมรูปภาพ

ฟอร์มการตัดสินใจ:

| ฟิลด์ | ประเภท | หมายเหตุ |
|---|---|---|
| approved_price | number | ราคาที่ตั้ง |
| condition_discount_pct | number | % ส่วนลดตามสภาพ |
| review_note | textarea | หมายเหตุ |
| rejection_reason | textarea | แสดงเมื่อเลือก "ปฏิเสธ" |

ปุ่ม: "อนุมัติ" / "ปฏิเสธ" → `POST /Appraisal/Review/{book_id}`

---

## 12) Finance (พนักงานการเงิน)

### `Queue.cshtml`
คิวการชำระเงินรอตรวจสอบ — [Authorize(Roles = "Finance,Admin")]

ตาราง: order_no / ลูกค้า / total_amount / payment_status badge / วันที่อัปโหลดหลักฐาน / ปุ่มตรวจสอบ

กรองตาม: payment_status (submitted / pending)

---

### `PaymentDetail.cshtml`
ตรวจสอบหลักฐานการชำระเงิน — [Authorize(Roles = "Finance,Admin")]

แสดง: order_no / รายการสินค้า / ยอดสุทธิ
แสดง: หลักฐาน (evidence_url รูปภาพ) / payer_name / transfer_amount / transfer_datetime

ฟอร์มตัดสินใจ:

| ฟิลด์ | ประเภท | หมายเหตุ |
|---|---|---|
| reject_reason | textarea | แสดงเมื่อเลือก "ปฏิเสธ" |

ปุ่ม: "อนุมัติ" / "ปฏิเสธ" → `POST /Finance/Verify/{payment_id}`

---

### `PromotionIndex.cshtml`
รายการโปรโมชันทั้งหมด — [Authorize(Roles = "Finance,Admin")]

ตาราง: promotion_code / ชื่อ / ประเภท / ส่วนลด / วันเริ่ม-สิ้นสุด / is_active toggle / ปุ่มแก้ไข

ปุ่ม: สร้างโปรโมชันใหม่

---

### `PromotionCreate.cshtml` / `PromotionEdit.cshtml`
ฟอร์มสร้าง/แก้ไขโปรโมชัน — [Authorize(Roles = "Finance,Admin")]

ส่วน "ข้อมูลโปรโมชัน":

| ฟิลด์ | ประเภท | หมายเหตุ |
|---|---|---|
| promotion_code | text | |
| promotion_name | text | |
| promotion_type | select | |
| description | textarea | |
| discount_type | select | |
| discount_value | number | |
| min_order_amount | number | |
| start_at | datetime-local | |
| end_at | datetime-local | |
| is_active | checkbox | |
| is_auto_apply | checkbox | |

ส่วน "กฎโปรโมชัน (promotion_rules)": ตารางเพิ่ม/ลบกฎแบบ dynamic

| ฟิลด์ | ประเภท | หมายเหตุ |
|---|---|---|
| rule_type | select | condition / book_scope / series_scope / category_scope |
| rule_operator | select | = / >= / IN |
| rule_value | text | ค่าที่ใช้ตรวจสอบ |
| series_name | text | สำหรับ series_scope |
| category_name | text | สำหรับ category_scope |

Action: `POST /Finance/PromotionCreate` หรือ `POST /Finance/PromotionEdit/{id}`

---

## 13) Shipping (ฝ่ายจัดส่ง)

### `Queue.cshtml`
คิวคำสั่งซื้อพร้อมจัดส่ง — [Authorize(Roles = "Shipping,Admin")]

ตาราง: order_no / ลูกค้า / ที่อยู่ (snap_province) / จำนวนรายการ / shipment_status badge / ปุ่มจัดการ

กรองตาม: shipment_status (pending / ready_to_pack / packed)

---

### `PackDetail.cshtml`
รายละเอียดการแพ็กและพิมพ์ใบจ่าหน้า — [Authorize(Roles = "Shipping,Admin")]

แสดง: ที่อยู่จัดส่งแบบเต็ม (snap_* fields) / รายการ order_items

ปุ่ม: "พิมพ์ใบจ่าหน้า" (เปิด print dialog) / "ยืนยันแพ็กแล้ว" → `POST /Shipping/ConfirmPacked/{shipment_id}`

---

### `UpdateTracking.cshtml`
กรอกเลขพัสดุและอัปเดตสถานะ — [Authorize(Roles = "Shipping,Admin")]

| ฟิลด์ | ประเภท | หมายเหตุ |
|---|---|---|
| carrier_name | text / select | ชื่อบริษัทขนส่ง |
| tracking_no | text | เลขติดตามพัสดุ |
| shipment_status | select | packed / shipped / delivered |

Action: `POST /Shipping/UpdateTracking/{shipment_id}`

---

## 14) Admin

### `Dashboard.cshtml`
สรุปภาพรวมระบบ — [Authorize(Roles = "Admin")]

Card สรุป:
- หนังสือรอตรวจสอบ (approval_status = pending)
- การชำระเงินรอตรวจสอบ (payment_status = submitted)
- คำสั่งซื้อรอจัดส่ง (order_status = payment_verified)
- ลูกค้าทั้งหมด / พนักงานทั้งหมด

---

### `CustomerList.cshtml`
รายการลูกค้าทั้งหมด — [Authorize(Roles = "Admin")]

ตาราง: display_name / email / phone / status badge / current_points / วันที่สมัคร / ปุ่มดูรายละเอียด

ค้นหา: ชื่อ / อีเมล / เบอร์

---

### `CustomerDetail.cshtml`
ข้อมูลลูกค้าและการจัดการ — [Authorize(Roles = "Admin")]

แสดงข้อมูลโปรไฟล์ทั้งหมด / ที่อยู่ / แต้ม

ปุ่ม: ระงับบัญชี / ปลดระงับ / ปรับแต้ม (manual adjust) → `POST /Admin/AdjustPoints/{customer_id}`

---

### `EmployeeList.cshtml`
รายการพนักงาน — [Authorize(Roles = "Admin")]

ตาราง: employee_code / full_name / Role / employment_status badge / hire_date / ปุ่มแก้ไข

---

### `EmployeeCreate.cshtml`
เพิ่มพนักงานใหม่ — [Authorize(Roles = "Admin")]

| ฟิลด์ | ประเภท | หมายเหตุ |
|---|---|---|
| username | text | |
| email | email | |
| password | password | |
| full_name | text | |
| employee_code | text | |
| role | select | Appraisal / Finance / Shipping / Admin |
| hire_date | date | |

Action: `POST /Admin/EmployeeCreate`

---

### `EmployeeEdit.cshtml`
แก้ไขข้อมูลพนักงาน — [Authorize(Roles = "Admin")]

| ฟิลด์ | ประเภท | หมายเหตุ |
|---|---|---|
| full_name | text | |
| role | select | เปลี่ยน Role ได้ |
| employment_status | select | active / inactive / resigned |
| resign_date | date | แสดงเมื่อ resigned |

Action: `POST /Admin/EmployeeEdit/{employee_id}`

---

## 15) สรุป Views ทั้งหมด

| Controller | Views | Role |
|---|---|---|
| Shared | _Layout, _LayoutStaff, _Navbar, _BookCard, _OrderStatusBadge, _Pagination, Error | ทุก Role |
| Account | Register, Login, AccessDenied | Anonymous |
| Customer | Profile, MyBooks | Customer |
| BookCatalog | Index, Detail | Anonymous |
| SellBook | Create, Edit | Customer |
| Cart | Index | Customer |
| Checkout | Index | Customer |
| Order | Index, Detail | Customer |
| Payment | Upload | Customer |
| Points | History | Customer |
| Appraisal | Queue, Review | Appraisal, Admin |
| Finance | Queue, PaymentDetail, PromotionIndex, PromotionCreate, PromotionEdit | Finance, Admin |
| Shipping | Queue, PackDetail, UpdateTracking | Shipping, Admin |
| Admin | Dashboard, CustomerList, CustomerDetail, EmployeeList, EmployeeCreate, EmployeeEdit | Admin |

**รวมทั้งหมด 40 Views** (รวม Shared Partials 7 ชิ้น)

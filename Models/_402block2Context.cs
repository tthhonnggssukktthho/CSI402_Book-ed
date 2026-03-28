using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace _66014444_Project.Models;

public partial class _402block2Context : DbContext
{
    public _402block2Context()
    {
    }

    public _402block2Context(DbContextOptions<_402block2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PointTransaction> PointTransactions { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<PromotionRule> PromotionRules { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }

    public virtual DbSet<SystemSetting> SystemSettings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3305;database=402block2;user=root;password=561286538911581299ZXzx_Heng45621012;sslmode=none;allowpublickeyretrieval=True", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.6.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PRIMARY");

            entity.ToTable("books");

            entity.HasIndex(e => e.ReviewedByEmployeeId, "FK_books_reviewer");

            entity.HasIndex(e => e.SellerCustomerId, "FK_books_seller");

            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("'pending'")
                .HasColumnName("approval_status")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ApprovedPrice)
                .HasPrecision(10, 2)
                .HasColumnName("approved_price");
            entity.Property(e => e.AuthorName)
                .HasMaxLength(150)
                .HasColumnName("author_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.BookDescription)
                .HasColumnType("text")
                .HasColumnName("book_description");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("category_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ConditionCode)
                .HasMaxLength(30)
                .HasColumnName("condition_code")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ConditionDiscountPct)
                .HasPrecision(5, 2)
                .HasColumnName("condition_discount_pct");
            entity.Property(e => e.ConditionNote)
                .HasMaxLength(255)
                .HasColumnName("condition_note")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .HasColumnName("image_url")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("isbn")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ProposedPrice)
                .HasPrecision(10, 2)
                .HasColumnName("proposed_price");
            entity.Property(e => e.PublishYear).HasColumnName("publish_year");
            entity.Property(e => e.PublisherName)
                .HasMaxLength(150)
                .HasColumnName("publisher_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.RejectionReason)
                .HasMaxLength(255)
                .HasColumnName("rejection_reason")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ReviewedAt)
                .HasColumnType("datetime")
                .HasColumnName("reviewed_at");
            entity.Property(e => e.ReviewedByEmployeeId).HasColumnName("reviewed_by_employee_id");
            entity.Property(e => e.SaleStatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("'draft'")
                .HasColumnName("sale_status")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SellerCustomerId).HasColumnName("seller_customer_id");
            entity.Property(e => e.SeriesName)
                .HasMaxLength(255)
                .HasColumnName("series_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Synopsis)
                .HasColumnType("text")
                .HasColumnName("synopsis");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.VolumeNo)
                .HasMaxLength(20)
                .HasColumnName("volume_no")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.ReviewedByEmployee).WithMany(p => p.Books)
                .HasForeignKey(d => d.ReviewedByEmployeeId)
                .HasConstraintName("FK_books_reviewer");

            entity.HasOne(d => d.SellerCustomer).WithMany(p => p.Books)
                .HasForeignKey(d => d.SellerCustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_books_seller");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PRIMARY");

            entity.ToTable("cart_items");

            entity.HasIndex(e => e.BookId, "FK_cart_items_books");

            entity.HasIndex(e => new { e.CustomerId, e.BookId }, "UQ_cart_items_cust_book").IsUnique();

            entity.Property(e => e.CartItemId).HasColumnName("cart_item_id");
            entity.Property(e => e.AddedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("added_at");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(10, 2)
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Book).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cart_items_books");

            entity.HasOne(d => d.Customer).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cart_items_customers");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PRIMARY");

            entity.ToTable("customers");

            entity.HasIndex(e => e.UserId, "UQ_customers_user").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.AddressLine1)
                .HasMaxLength(255)
                .HasColumnName("address_line1")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(255)
                .HasColumnName("address_line2")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CurrentPoints).HasColumnName("current_points");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .HasColumnName("display_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.District)
                .HasMaxLength(100)
                .HasColumnName("district")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .HasColumnName("postal_code")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Province)
                .HasMaxLength(100)
                .HasColumnName("province")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ReceiverName)
                .HasMaxLength(100)
                .HasColumnName("receiver_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ReceiverPhone)
                .HasMaxLength(20)
                .HasColumnName("receiver_phone")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'active'")
                .HasColumnName("status")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Subdistrict)
                .HasMaxLength(100)
                .HasColumnName("subdistrict")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_customers_users");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PRIMARY");

            entity.ToTable("employees");

            entity.HasIndex(e => e.CreatedByAdminId, "FK_employees_created_by");

            entity.HasIndex(e => e.EmployeeCode, "UQ_employees_code").IsUnique();

            entity.HasIndex(e => e.UserId, "UQ_employees_user").IsUnique();

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.AddressLine1)
                .HasMaxLength(255)
                .HasColumnName("address_line1")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(255)
                .HasColumnName("address_line2")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedByAdminId).HasColumnName("created_by_admin_id");
            entity.Property(e => e.District)
                .HasMaxLength(100)
                .HasColumnName("district")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(30)
                .HasColumnName("employee_code")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.EmploymentStatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("'active'")
                .HasColumnName("employment_status")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasColumnName("full_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasColumnName("role")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .HasColumnName("postal_code")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Province)
                .HasMaxLength(100)
                .HasColumnName("province")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ResignDate).HasColumnName("resign_date");
            entity.Property(e => e.Subdistrict)
                .HasMaxLength(100)
                .HasColumnName("subdistrict")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.CreatedByAdmin).WithMany(p => p.EmployeeCreatedByAdmins)
                .HasForeignKey(d => d.CreatedByAdminId)
                .HasConstraintName("FK_employees_created_by");

            entity.HasOne(d => d.User).WithOne(p => p.EmployeeUser)
                .HasForeignKey<Employee>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employees_users");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("orders");

            entity.HasIndex(e => e.CustomerId, "FK_orders_customers");

            entity.HasIndex(e => e.PromotionId, "FK_orders_promotions");

            entity.HasIndex(e => e.OrderNo, "UQ_orders_no").IsUnique();

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CancelReason)
                .HasMaxLength(255)
                .HasColumnName("cancel_reason")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CancelledAt)
                .HasColumnType("datetime")
                .HasColumnName("cancelled_at");
            entity.Property(e => e.ConditionDiscountAmount)
                .HasPrecision(10, 2)
                .HasColumnName("condition_discount_amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.OrderNo)
                .HasMaxLength(30)
                .HasColumnName("order_no")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(30)
                .HasDefaultValueSql("'pending_payment'")
                .HasColumnName("order_status")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PaymentDueAt)
                .HasColumnType("datetime")
                .HasColumnName("payment_due_at");
            entity.Property(e => e.PointsDiscountAmount)
                .HasPrecision(10, 2)
                .HasColumnName("points_discount_amount");
            entity.Property(e => e.PointsEarned).HasColumnName("points_earned");
            entity.Property(e => e.PointsUsed).HasColumnName("points_used");
            entity.Property(e => e.PreviousStatus)
                .HasMaxLength(30)
                .HasColumnName("previous_status")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PromotionDiscountAmount)
                .HasPrecision(10, 2)
                .HasColumnName("promotion_discount_amount");
            entity.Property(e => e.PromotionId).HasColumnName("promotion_id");
            entity.Property(e => e.ShippingFee)
                .HasPrecision(10, 2)
                .HasColumnName("shipping_fee");
            entity.Property(e => e.SnapAddressLine1)
                .HasMaxLength(255)
                .HasColumnName("snap_address_line1")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SnapAddressLine2)
                .HasMaxLength(255)
                .HasColumnName("snap_address_line2")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SnapDistrict)
                .HasMaxLength(100)
                .HasColumnName("snap_district")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SnapPostalCode)
                .HasMaxLength(10)
                .HasColumnName("snap_postal_code")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SnapProvince)
                .HasMaxLength(100)
                .HasColumnName("snap_province")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SnapReceiverName)
                .HasMaxLength(100)
                .HasColumnName("snap_receiver_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SnapReceiverPhone)
                .HasMaxLength(20)
                .HasColumnName("snap_receiver_phone")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SnapSubdistrict)
                .HasMaxLength(100)
                .HasColumnName("snap_subdistrict")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SubtotalAmount)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal_amount");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(10, 2)
                .HasColumnName("total_amount");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_orders_customers");

            entity.HasOne(d => d.Promotion).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK_orders_promotions");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PRIMARY");

            entity.ToTable("order_items");

            entity.HasIndex(e => e.BookId, "FK_order_items_books");

            entity.HasIndex(e => e.OrderId, "FK_order_items_orders");

            entity.HasIndex(e => e.SellerCustomerId, "FK_order_items_seller");

            entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.BookTitleSnapshot)
                .HasMaxLength(255)
                .HasColumnName("book_title_snapshot")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ConditionCodeSnap)
                .HasMaxLength(30)
                .HasColumnName("condition_code_snap")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DiscountAmount)
                .HasPrecision(10, 2)
                .HasColumnName("discount_amount");
            entity.Property(e => e.NetAmount)
                .HasPrecision(10, 2)
                .HasColumnName("net_amount");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.SellerCustomerId).HasColumnName("seller_customer_id");
            entity.Property(e => e.SeriesNameSnapshot)
                .HasMaxLength(255)
                .HasColumnName("series_name_snapshot")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(10, 2)
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Book).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_order_items_books");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_order_items_orders");

            entity.HasOne(d => d.SellerCustomer).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.SellerCustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_order_items_seller");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PRIMARY");

            entity.ToTable("payments");

            entity.HasIndex(e => e.OrderId, "FK_payments_orders");

            entity.HasIndex(e => e.VerifiedByEmployeeId, "FK_payments_verifier");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EvidenceUploadedAt)
                .HasColumnType("datetime")
                .HasColumnName("evidence_uploaded_at");
            entity.Property(e => e.EvidenceUrl)
                .HasMaxLength(500)
                .HasColumnName("evidence_url")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PayerName)
                .HasMaxLength(100)
                .HasColumnName("payer_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(30)
                .HasDefaultValueSql("'bank_transfer'")
                .HasColumnName("payment_method")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(30)
                .HasDefaultValueSql("'pending'")
                .HasColumnName("payment_status")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.RejectReason)
                .HasMaxLength(255)
                .HasColumnName("reject_reason")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.TransferAmount)
                .HasPrecision(10, 2)
                .HasColumnName("transfer_amount");
            entity.Property(e => e.TransferDatetime)
                .HasColumnType("datetime")
                .HasColumnName("transfer_datetime");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.VerifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("verified_at");
            entity.Property(e => e.VerifiedByEmployeeId).HasColumnName("verified_by_employee_id");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_payments_orders");

            entity.HasOne(d => d.VerifiedByEmployee).WithMany(p => p.Payments)
                .HasForeignKey(d => d.VerifiedByEmployeeId)
                .HasConstraintName("FK_payments_verifier");
        });

        modelBuilder.Entity<PointTransaction>(entity =>
        {
            entity.HasKey(e => e.PointTxnId).HasName("PRIMARY");

            entity.ToTable("point_transactions");

            entity.HasIndex(e => e.CreatedByUserId, "FK_point_txn_created_by");

            entity.HasIndex(e => e.CustomerId, "FK_point_txn_customers");

            entity.HasIndex(e => e.OrderId, "FK_point_txn_orders");

            entity.Property(e => e.PointTxnId).HasColumnName("point_txn_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(20)
                .HasColumnName("transaction_type")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.PointTransactions)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK_point_txn_created_by");

            entity.HasOne(d => d.Customer).WithMany(p => p.PointTransactions)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_point_txn_customers");

            entity.HasOne(d => d.Order).WithMany(p => p.PointTransactions)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_point_txn_orders");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PRIMARY");

            entity.ToTable("promotions");

            entity.HasIndex(e => e.CreatedByEmployeeId, "FK_promotions_creator");

            entity.HasIndex(e => e.PromotionCode, "UQ_promotions_code").IsUnique();

            entity.Property(e => e.PromotionId).HasColumnName("promotion_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedByEmployeeId).HasColumnName("created_by_employee_id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DiscountType)
                .HasMaxLength(20)
                .HasColumnName("discount_type")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DiscountValue)
                .HasPrecision(10, 2)
                .HasColumnName("discount_value");
            entity.Property(e => e.EndAt)
                .HasColumnType("datetime")
                .HasColumnName("end_at");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.IsAutoApply).HasColumnName("is_auto_apply");
            entity.Property(e => e.MinItemQty).HasColumnName("min_item_qty");
            entity.Property(e => e.MinOrderAmount)
                .HasPrecision(10, 2)
                .HasColumnName("min_order_amount");
            entity.Property(e => e.PromotionCode)
                .HasMaxLength(50)
                .HasColumnName("promotion_code")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PromotionName)
                .HasMaxLength(150)
                .HasColumnName("promotion_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PromotionType)
                .HasMaxLength(30)
                .HasColumnName("promotion_type")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.StartAt)
                .HasColumnType("datetime")
                .HasColumnName("start_at");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.CreatedByEmployee).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.CreatedByEmployeeId)
                .HasConstraintName("FK_promotions_creator");
        });

        modelBuilder.Entity<PromotionRule>(entity =>
        {
            entity.HasKey(e => e.RuleId).HasName("PRIMARY");

            entity.ToTable("promotion_rules");

            entity.HasIndex(e => e.BookId, "FK_promotion_rules_book");

            entity.HasIndex(e => e.PromotionId, "FK_promotion_rules_promo");

            entity.Property(e => e.RuleId).HasColumnName("rule_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("category_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.PromotionId).HasColumnName("promotion_id");
            entity.Property(e => e.RuleOperator)
                .HasMaxLength(20)
                .HasColumnName("rule_operator")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.RuleType)
                .HasMaxLength(30)
                .HasColumnName("rule_type")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.RuleValue)
                .HasMaxLength(255)
                .HasColumnName("rule_value")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SeriesName)
                .HasMaxLength(255)
                .HasColumnName("series_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.Book).WithMany(p => p.PromotionRules)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_promotion_rules_book");

            entity.HasOne(d => d.Promotion).WithMany(p => p.PromotionRules)
                .HasForeignKey(d => d.PromotionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_promotion_rules_promo");
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(e => e.ShipmentId).HasName("PRIMARY");

            entity.ToTable("shipments");

            entity.HasIndex(e => e.PackedByEmployeeId, "FK_shipments_packer");

            entity.HasIndex(e => e.OrderId, "UQ_shipments_order").IsUnique();

            entity.Property(e => e.ShipmentId).HasColumnName("shipment_id");
            entity.Property(e => e.CarrierName)
                .HasMaxLength(100)
                .HasColumnName("carrier_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeliveredAt)
                .HasColumnType("datetime")
                .HasColumnName("delivered_at");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PackedByEmployeeId).HasColumnName("packed_by_employee_id");
            entity.Property(e => e.ShipmentStatus)
                .HasMaxLength(30)
                .HasDefaultValueSql("'pending'")
                .HasColumnName("shipment_status")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ShippedAt)
                .HasColumnType("datetime")
                .HasColumnName("shipped_at");
            entity.Property(e => e.ShippingLabelPrintedAt)
                .HasColumnType("datetime")
                .HasColumnName("shipping_label_printed_at");
            entity.Property(e => e.TrackingNo)
                .HasMaxLength(100)
                .HasColumnName("tracking_no")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Order).WithOne(p => p.Shipment)
                .HasForeignKey<Shipment>(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_shipments_orders");

            entity.HasOne(d => d.PackedByEmployee).WithMany(p => p.Shipments)
                .HasForeignKey(d => d.PackedByEmployeeId)
                .HasConstraintName("FK_shipments_packer");
        });

        modelBuilder.Entity<SystemSetting>(entity =>
        {
            entity.HasKey(e => e.SettingKey).HasName("PRIMARY");

            entity.ToTable("system_settings");

            entity.Property(e => e.SettingKey)
                .HasMaxLength(100)
                .HasColumnName("setting_key")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SettingValue)
                .HasMaxLength(255)
                .HasColumnName("setting_value")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ_users_email").IsUnique();

            entity.HasIndex(e => e.Username, "UQ_users_username").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.LastLoginAt)
                .HasColumnType("datetime")
                .HasColumnName("last_login_at");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserType)
                .HasMaxLength(20)
                .HasColumnName("user_type")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

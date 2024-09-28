using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DoAnCoSo.ModelView;

namespace DoAnCoSo.Models;

public partial class DataDoAnCoSoContext : DbContext
{
    public DataDoAnCoSoContext()
    {
    }

    public DataDoAnCoSoContext(DbContextOptions<DataDoAnCoSoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Shipper> Shippers { get; set; }

    public virtual DbSet<TransactStatus> TransactStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=VINH\\SQLEXPRESS;Initial Catalog=DataDoAnCoSo;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK_Account");
            entity.Property(e => e.AccountId).HasColumnType("int").UseIdentityColumn();
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.PasswordAcc).HasMaxLength(50);
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Salt)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UserNameAcc).HasMaxLength(150);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Account_Roles");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");
            entity.Property(e => e.CartId).HasColumnType("int").UseIdentityColumn();
            entity.Property(e => e.ProImage).HasMaxLength(100);
            entity.Property(e => e.ProName).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.Carts)
                .HasForeignKey(d => d.CartId);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CatId);
            entity.Property(e => e.CatId).HasColumnType("int").UseIdentityColumn();
            entity.ToTable("Category");

            entity.Property(e => e.CatName).HasMaxLength(50);
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.ToTable("Contact");
            entity.Property(e => e.ContactId).HasColumnType("int").UseIdentityColumn();
            entity.Property(e => e.ContactId).HasColumnName("ContactID");
            entity.Property(e => e.CusEmail)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CusMess).HasMaxLength(250);
            entity.Property(e => e.CusName).HasMaxLength(100);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CusId);
            entity.Property(e => e.CusId).HasColumnType("int").UseIdentityColumn();
            entity.ToTable("Customer");

            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.CusEmail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CusName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CusPassword)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
               .HasMaxLength(150)
               .IsUnicode(false);
            entity.Property(e => e.Salt)
                .HasMaxLength(10)
                .IsFixedLength();

        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK_Location");
            entity.Property(e => e.LocationId).HasColumnType("int").UseIdentityColumn();
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.NameWithType).HasMaxLength(150);
            entity.Property(e => e.PathWithType).HasMaxLength(150);
            entity.Property(e => e.Type).HasMaxLength(150);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderId).HasColumnType("int").UseIdentityColumn();
           
            entity.Property(e => e.OderDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentType).HasMaxLength(50);
            entity.Property(e => e.ShipType).HasMaxLength(50);
            entity.Property(e => e.TotalMoney).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CusAddress).HasMaxLength(150);
            entity.Property(e => e.CusFullName).HasMaxLength(150);
            entity.Property(e => e.PayShip).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CusPhone).HasMaxLength(50);
            
            entity.HasOne(d => d.Cus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CusId)
                .HasConstraintName("FK_Orders_Customer");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_Orders_TransactStatus");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentType).HasMaxLength(50);
            entity.Property(e => e.ProImage).HasMaxLength(100);
            entity.Property(e => e.ProName).HasMaxLength(50);
            entity.Property(e => e.StatusOrderDetail).HasMaxLength(50);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderDetail_Orders");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProId);
            entity.Property(e => e.ProId).HasColumnType("int").UseIdentityColumn();
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.MeetaKey).HasMaxLength(100);
            entity.Property(e => e.MetaDesc).HasMaxLength(100);
            entity.Property(e => e.ProImage).HasMaxLength(100);
            entity.Property(e => e.ProName).HasMaxLength(250);
            entity.Property(e => e.ShortDes).HasMaxLength(50);
            entity.Property(e => e.ProPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Cat).WithMany(p => p.Products)
                .HasForeignKey(d => d.CatId)
                .HasConstraintName("FK_Products_Category");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).HasColumnType("int").UseIdentityColumn();
            entity.Property(e => e.DesRole).HasMaxLength(50);
            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<Shipper>(entity =>
        {
            entity.HasKey(e => e.ShipperId).HasName("Pk_Shipper");

            entity.ToTable("Shipper");

            entity.Property(e => e.Company).HasMaxLength(50);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ShipperName).HasMaxLength(50);
        });

        modelBuilder.Entity<TransactStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK_Status");

            entity.ToTable("TransactStatus");

            entity.Property(e => e.Status).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

public DbSet<DoAnCoSo.ModelView.RegisterVM> RegisterVM { get; set; } = default!;

public DbSet<DoAnCoSo.ModelView.LoginViewModel> LoginViewModel { get; set; } = default!;
}

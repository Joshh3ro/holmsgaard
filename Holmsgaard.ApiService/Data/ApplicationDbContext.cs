using Holmsgaard.ApiService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Holmsgaard.ApiService.Data;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<TimeRegistration> TimeRegistrations => Set<TimeRegistration>();
    public DbSet<WarehousePurchase> WarehousePurchases => Set<WarehousePurchase>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.HourlyRate).HasPrecision(18, 2);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasOne<Customer>()
                  .WithMany()
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Sku).IsRequired().HasMaxLength(50);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RowVersion).IsRowVersion();
            entity.Property(e => e.Category)
                  .HasConversion<string>()
                  .HasMaxLength(50);
        });

        modelBuilder.Entity<TimeRegistration>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Hours).HasPrecision(18, 2);
            entity.Property(e => e.Note).HasMaxLength(1000);
            entity.HasOne<Employee>()
                  .WithMany()
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Activity>()
                  .WithMany()
                  .HasForeignKey(e => e.ActivityId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<WarehousePurchase>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPriceAtPurchase).HasPrecision(18, 2);
            entity.Property(e => e.Supplier).HasMaxLength(200);
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.HasOne<Product>()
                  .WithMany()
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

using BusinessOps.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessOps.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Shipment> Shipments => Set<Shipment>();
    public DbSet<SupportIssue> SupportIssues => Set<SupportIssue>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Sku)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(p => p.Sku)
                .IsUnique();

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.Category)
                .HasMaxLength(100);

            entity.Property(p => p.UnitCost)
                .HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.OrderNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(o => o.OrderNumber)
                .IsUnique();

            entity.Property(o => o.CustomerName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(o => o.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(o => o.Notes)
                .HasMaxLength(1000);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(oi => oi.Id);

            entity.Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)");

            entity.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.Carrier)
                .HasMaxLength(100);

            entity.Property(s => s.TrackingNumber)
                .HasMaxLength(100);

            entity.Property(s => s.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(s => s.DelayReason)
                .HasMaxLength(1000);

            entity.HasOne(s => s.Order)
                .WithMany(o => o.Shipments)
                .HasForeignKey(s => s.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SupportIssue>(entity =>
        {
            entity.HasKey(si => si.Id);

            entity.Property(si => si.IssueType)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(si => si.Priority)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(si => si.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(si => si.Description)
                .IsRequired()
                .HasMaxLength(2000);

            entity.HasOne(si => si.RelatedOrder)
                .WithMany(o => o.SupportIssues)
                .HasForeignKey(si => si.RelatedOrderId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
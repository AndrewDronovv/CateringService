using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(i => i.Id)
            .HasColumnName("OrderId")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(o => o.OrderDate)
            .IsRequired();

        builder.Property(o => o.DeliveryDate)
            .IsRequired();

        builder.Property(o => o.TotalPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(o => o.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Supplier)
            .WithMany(s => s.Orders)
            .HasForeignKey(o => o.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Delivery)
            .WithMany(d => d.Orders)
            .HasForeignKey(o => o.DeliveryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData
        (
            new Order
            {
                Id = 1,
                OrderDate = new DateTime(2025, 4, 10),
                DeliveryDate = new DateTime(2025, 4, 12),
                TotalPrice = 250.00m,
                Status = "Completed",
                CustomerId = 1,
                SupplierId = 1,
                DeliveryId = 1
            },
            new Order
            {
                Id = 2,
                OrderDate = new DateTime(2025, 4, 11),
                DeliveryDate = new DateTime(2025, 4, 13),
                TotalPrice = 150.75m,
                Status = "Pending",
                CustomerId = 2,
                SupplierId = 2,
                DeliveryId = 2
            },
            new Order
            {
                Id = 3,
                OrderDate = new DateTime(2025, 4, 12),
                DeliveryDate = new DateTime(2025, 4, 14),
                TotalPrice = 300.50m,
                Status = "Cancelled",
                CustomerId = 3,
                SupplierId = 3,
                DeliveryId = 3
            }
        );
    }
}
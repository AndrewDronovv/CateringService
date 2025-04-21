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

        builder.Property(d => d.Id)
            .HasColumnName("OrderId")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

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
                Id = Ulid.Parse("01H5QJ3DZP8N3A1EQNHQZK7GTT"),
                OrderDate = new DateTime(2025, 4, 10),
                DeliveryDate = new DateTime(2025, 4, 12),
                TotalPrice = 250.00m,
                Status = "Completed",
                CustomerId = Ulid.Parse("01H5QJ37V03WH5TXE2N1AW3JF9"),
                SupplierId = Ulid.Parse("01H5QJ6PTMVRFZT58GQX902JC4"),
                DeliveryId = Ulid.Parse("01H5QJ399WTKN11Z9FMB02WT62")
            },
            new Order
            {
                Id = Ulid.Parse("01H5QJ3E1TZPGJ82MMZ20WX44Z"),
                OrderDate = new DateTime(2025, 4, 11),
                DeliveryDate = new DateTime(2025, 4, 13),
                TotalPrice = 150.75m,
                Status = "Pending",
                CustomerId = Ulid.Parse("01H5QJ38KGWM2N56TFH99WQZ03"),
                SupplierId = Ulid.Parse("01H5QJ6PVB8FYN4QXMR3T7JC9A"),
                DeliveryId = Ulid.Parse("01H5QJ39VRZ2AN3YC94PM5FMPA")
            },
            new Order
            {
                Id = Ulid.Parse("01H5QJ3E3P7D4X8KVT4X30PKKQ"),
                OrderDate = new DateTime(2025, 4, 12),
                DeliveryDate = new DateTime(2025, 4, 14),
                TotalPrice = 300.50m,
                Status = "Cancelled",
                CustomerId = Ulid.Parse("01H5QJ391M8PVG6ZWPK4GTN0D8"),
                SupplierId = Ulid.Parse("01H5QJ6PX4FTQY8KZVW9JMBT96"),
                DeliveryId = Ulid.Parse("01H5QJ3A8D7V2GPF2K4K3WH5C4")
            }
        );
    }
}
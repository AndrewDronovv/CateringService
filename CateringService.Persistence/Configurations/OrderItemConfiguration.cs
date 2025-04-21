using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(oi => oi.Id);

        builder.Property(d => d.Id)
            .HasColumnName("OrderItemId")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Property(oi => oi.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(oi => oi.Dish)
            .WithMany()
            .HasForeignKey(oi => oi.DishId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData
        (
            new OrderItem
            {
                Id = Ulid.Parse("01H5QJ3E5929D8TFHK4M4PK0YE"),
                Quantity = 2,
                Price = 25.00m,
                OrderId = Ulid.Parse("01H5QJ3DZP8N3A1EQNHQZK7GTT"),
                DishId = Ulid.Parse("01GRQX9AYRHCA5Y5X3GPKPZ92P")
            },
            new OrderItem
            {
                Id = Ulid.Parse("01H5QJ3E72PFV0T3XN92K4W59V"),
                Quantity = 1,
                Price = 15.50m,
                OrderId = Ulid.Parse("01H5QJ3E1TZPGJ82MMZ20WX44Z"),
                DishId = Ulid.Parse("01GRQX9AYRHCA5Y5X3GPKPZ93Q")
            },
            new OrderItem
            {
                Id = Ulid.Parse("01H5QJ6P1YKRV9FX54Z0W3PJAY"),
                Quantity = 3,
                Price = 45.75m,
                OrderId = Ulid.Parse("01H5QJ3E3P7D4X8KVT4X30PKKQ"),
                DishId = Ulid.Parse("01H5PY6RF4WKFCR9VCMY2QNFGP")
            }
        );
    }
}

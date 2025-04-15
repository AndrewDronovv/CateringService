using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Id)
            .HasColumnName("OrderItemId")
            .ValueGeneratedOnAdd()
            .IsRequired();

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
                Id = 1,
                Quantity = 2,
                Price = 25.00m,
                OrderId = 1,
                DishId = 1
            },
            new OrderItem
            {
                Id = 2,
                Quantity = 1,
                Price = 15.50m,
                OrderId = 2,
                DishId = 2
            },
            new OrderItem
            {
                Id = 3,
                Quantity = 3,
                Price = 45.75m,
                OrderId = 3,
                DishId = 3
            }
        );
    }
}

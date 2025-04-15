using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public class PromotionConfigurtaion : IEntityTypeConfiguration<Promotion>
{
    public void Configure(EntityTypeBuilder<Promotion> builder)
    {
        builder.ToTable("Promotions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("PromotionId")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(p => p.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.DiscountValue)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.Condition)
            .HasMaxLength(200);

        builder.Property(p => p.StartDate)
            .IsRequired();

        builder.Property(p => p.EndDate)
            .IsRequired();

        builder.HasOne(p => p.Supplier)
            .WithMany(s => s.Promotions)
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData
        (
            new Promotion
            {
                Id = 1,
                Type = "Percentage",
                DiscountValue = 15.00m,
                Condition = "Minimum order $100",
                StartDate = new DateTime(2025, 4, 1),
                EndDate = new DateTime(2025, 4, 15),
                SupplierId = 1
            },
            new Promotion
            {
                Id = 2,
                Type = "Fixed Amount",
                DiscountValue = 20.00m,
                Condition = "For first-time customers",
                StartDate = new DateTime(2025, 4, 10),
                EndDate = new DateTime(2025, 4, 20),
                SupplierId = 2
            },
            new Promotion
            {
                Id = 3,
                Type = "Free Delivery",
                DiscountValue = 0.00m,
                Condition = "For orders over $50",
                StartDate = new DateTime(2025, 4, 5),
                EndDate = new DateTime(2025, 4, 25),
                SupplierId = 3
            }
        );
    }
}

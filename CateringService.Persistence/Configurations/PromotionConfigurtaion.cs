using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class PromotionConfigurtaion : IEntityTypeConfiguration<Promotion>
{
    public void Configure(EntityTypeBuilder<Promotion> builder)
    {
        builder.ToTable("Promotions");

        builder.HasKey(p => p.Id);

        builder.Property(d => d.Id)
            .HasColumnName("PromotionId")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

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
                Id = Ulid.Parse("01H5QJ6P88F6YXPNKJX42VFYB5"),
                Type = "Percentage",
                DiscountValue = 15.00m,
                Condition = "Minimum order $100",
                StartDate = new DateTime(2025, 4, 1),
                EndDate = new DateTime(2025, 4, 15),
                SupplierId = Ulid.Parse("01H5QJ6PTMVRFZT58GQX902JC4")
            },
            new Promotion
            {
                Id = Ulid.Parse("01H5QJ6PCZJ70AW3MMFGXK5TBQ"),
                Type = "Fixed Amount",
                DiscountValue = 20.00m,
                Condition = "For first-time customers",
                StartDate = new DateTime(2025, 4, 10),
                EndDate = new DateTime(2025, 4, 20),
                SupplierId = Ulid.Parse("01H5QJ6PVB8FYN4QXMR3T7JC9A")
            },
            new Promotion
            {
                Id = Ulid.Parse("01H5QJ6PFAWWNG1T52BZ20RQFX"),
                Type = "Free Delivery",
                DiscountValue = 0.00m,
                Condition = "For orders over $50",
                StartDate = new DateTime(2025, 4, 5),
                EndDate = new DateTime(2025, 4, 25),
                SupplierId = Ulid.Parse("01H5QJ6PX4FTQY8KZVW9JMBT96")
            }
        );
    }
}

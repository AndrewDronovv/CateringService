using CateringService.Domain.Entities.Approved;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class DishConfiguration : IEntityTypeConfiguration<Dish>
{
    public void Configure(EntityTypeBuilder<Dish> builder)
    {
        builder.ToTable("Dishes");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("Id")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Description)
            .HasMaxLength(500);

        builder.Property(d => d.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(d => d.Ingredients)
            .HasMaxLength(500);

        builder.Property(d => d.Weight)
            .IsRequired();

        builder.Property(d => d.ImageUrl)
            .HasMaxLength(200);

        builder.Property(d => d.IsAvailable)
            .IsRequired();

        builder.Property(d => d.Allergens)
            .HasMaxLength(500);

        builder.Property(d => d.PortionSize)
            .HasMaxLength(150);

        builder.Property(d => d.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasOne(d => d.MenuCategory)
            .WithMany(ms => ms.Dishes)
            .HasForeignKey(d => d.MenuCategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(d => d.Id);
    }
}
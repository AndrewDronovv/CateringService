using CateringService.Domain.Entities.Approved;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class MenuCategoryConfiguration : IEntityTypeConfiguration<MenuCategory>
{
    public void Configure(EntityTypeBuilder<MenuCategory> builder)
    {
        builder.ToTable("MenuCategories");

        builder.HasKey(mc => mc.Id);

        builder.Property(mc => mc.Id)
            .HasColumnName("Id")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

        builder.Property(mc => mc.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(mc => mc.Description)
            .HasMaxLength(500);

        builder.Property(mc => mc.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasMany(mc => mc.Dishes)
            .WithOne(d => d.MenuCategory)
            .HasForeignKey(d => d.MenuCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(mc => mc.SupplierId)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );
    }
}
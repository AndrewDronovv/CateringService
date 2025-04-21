using CateringService.Domain.Entities;
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
            .HasColumnName("MenuCategoryId")
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

        builder.HasOne(mc => mc.Supplier)
            .WithMany(s => s.MenuCategories)
            .HasForeignKey(mc => mc.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(mc => mc.Dishes)
            .WithOne(d => d.MenuCategory)
            .HasForeignKey(d => d.MenuCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData
        (
            new MenuCategory
            {
                Id = Ulid.Parse("01H5QJ3DHBM8J6AW04FKPJP5VV"),
                Name = "Appetizers",
                Description = "Start your meal with our delightful appetizers.",
                CreatedAt = new DateTime(2025, 04, 20, 10, 0, 0),
                SupplierId = Ulid.Parse("01H5QJ6PTMVRFZT58GQX902JC4")
            },
            new MenuCategory
            {
                Id = Ulid.Parse("01H5QJ3DJ22VXVG28Q0RYMNQEY"),
                Name = "Main Courses",
                Description = "Delicious main courses to satisfy your hunger.",
                CreatedAt = new DateTime(2025, 04, 20, 12, 0, 0),
                SupplierId = Ulid.Parse("01H5QJ6PVB8FYN4QXMR3T7JC9A")
            },
            new MenuCategory
            {
                Id = Ulid.Parse("01H5QJ3DR6R35WTKTPGFPJ89JC"),
                Name = "Desserts",
                Description = "End your meal with our sweet desserts.",
                CreatedAt = new DateTime(2025, 04, 20, 14, 0, 0),
                SupplierId = Ulid.Parse("01H5QJ6PX4FTQY8KZVW9JMBT96")
            }
        );
    }
}
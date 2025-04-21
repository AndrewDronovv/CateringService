using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public class MenuCategoryConfiguration : IEntityTypeConfiguration<MenuCategory>
{
    public void Configure(EntityTypeBuilder<MenuCategory> builder)
    {
        builder.ToTable("MenuSections");

        builder.HasKey(ms => ms.Id);

        builder.Property(d => d.Id)
            .HasColumnName("MenuSectionId")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

        builder.Property(ms => ms.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(ms => ms.Supplier)
            .WithMany(s => s.MenuSections)
            .HasForeignKey(ms => ms.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(ms => ms.Dishes)
            .WithOne(d => d.MenuCategory)
            .HasForeignKey(d => d.MenuCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData
        (
            new MenuCategory
            {
                Id = Ulid.Parse("01H5QJ3DHBM8J6AW04FKPJP5VV"),
                Name = "Appetizers",
                SupplierId = Ulid.Parse("01H5QJ6PTMVRFZT58GQX902JC4")
            },
            new MenuCategory
            {
                Id = Ulid.Parse("01H5QJ3DJ22VXVG28Q0RYMNQEY"),
                Name = "Main Courses",
                SupplierId = Ulid.Parse("01H5QJ6PVB8FYN4QXMR3T7JC9A")
            },
            new MenuCategory
            {
                Id = Ulid.Parse("01H5QJ3DR6R35WTKTPGFPJ89JC"),
                Name = "Desserts",
                SupplierId = Ulid.Parse("01H5QJ6PX4FTQY8KZVW9JMBT96")
            }
        );
    }
}

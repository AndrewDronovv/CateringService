using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public class MenuSectionConfiguration : IEntityTypeConfiguration<MenuSection>
{
    public void Configure(EntityTypeBuilder<MenuSection> builder)
    {
        builder.ToTable("MenuSections");

        builder.HasKey(ms => ms.Id);

        builder.Property(i => i.Id)
            .HasColumnName("MenuSectionId")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(ms => ms.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(ms => ms.Supplier)
            .WithMany(s => s.MenuSections)
            .HasForeignKey(ms => ms.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(ms => ms.Dishes)
            .WithOne(d => d.MenuSection)
            .HasForeignKey(d => d.MenuSectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData
        (
            new MenuSection
            {
                Id = 1,
                Name = "Appetizers",
                SupplierId = 1
            },
            new MenuSection
            {
                Id = 2,
                Name = "Main Courses",
                SupplierId = 2
            },
            new MenuSection
            {
                Id = 3,
                Name = "Desserts",
                SupplierId = 3
            }
        );
    }
}

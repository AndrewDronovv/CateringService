using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("SupplierId")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Description)
            .HasMaxLength(500)
            .HasComment("Описание поставщика");

        builder.Property(s => s.Logo)
            .HasMaxLength(200)
            .HasComment("Ссылка на логотип поставщика");

        builder.Property(s => s.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.WorkingHours)
            .IsRequired();

        builder.Property(s => s.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasMany(s => s.Dishes)
            .WithOne(d => d.Supplier) 
            .HasForeignKey(d => d.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.MenuSections)
            .WithOne(ms => ms.Supplier)
            .HasForeignKey(ms => ms.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.Promotions)
            .WithOne(p => p.Supplier) 
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.Invoices)
            .WithOne(i => i.Supplier)
            .HasForeignKey(i => i.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData
        (
            new Supplier
            {
                Id = 1,
                Name = "Fresh Produce Supplier",
                Description = "Поставщик свежих продуктов для ресторанов",
                Logo = "https://example.com/logo1.png",
                Phone = "+1234567890",
                Email = "contact@freshproduce.com",
                WorkingHours = 8,
                IsActive = true
            },
            new Supplier
            {
                Id = 2,
                Name = "Global Catering Supplies",
                Description = "Глобальный поставщик кейтерингового оборудования",
                Logo = "https://example.com/logo2.png",
                Phone = "+0987654321",
                Email = "info@globalcatering.com",
                WorkingHours = 10,
                IsActive = true
            },
            new Supplier
            {
                Id = 3,
                Name = "Organic Goods Co.",
                Description = "Поставщик органических продуктов питания",
                Logo = "https://example.com/logo3.png",
                Phone = "+1122334455",
                Email = "sales@organicgoods.com",
                WorkingHours = 6,
                IsActive = false
            }
        );
    }
}

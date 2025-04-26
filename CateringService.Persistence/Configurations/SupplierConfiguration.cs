using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");

        builder.HasKey(s => s.Id);

        builder.Property(d => d.Id)
            .HasColumnName("SupplierId")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

        builder.Property(s => s.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.ContactName)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("");

        builder.Property(s => s.TaxNumber)
            .IsRequired()
            .HasMaxLength(12);

        builder.Property(s => s.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasMany(s => s.Dishes)
            .WithOne(d => d.Supplier)
            .HasForeignKey(d => d.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.MenuCategories)
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
                Id = Ulid.Parse("01H5QJ6PTMVRFZT58GQX902JC4"),
                CompanyName = "Fresh Produce Supplier",
                ContactName = "John Doe",
                TaxNumber = "123456789",
                Phone = "+1234567890",
                Address = "123 Market Street, City A",
            },
            new Supplier
            {
                Id = Ulid.Parse("01H5QJ6PVB8FYN4QXMR3T7JC9A"),
                CompanyName = "Global Catering Supplies",
                ContactName = "Jane Smith",
                TaxNumber = "987654321",
                Phone = "+0987654321",
                Address = "456 Business Blvd, City B",
            },
            new Supplier
            {
                Id = Ulid.Parse("01H5QJ6PX4FTQY8KZVW9JMBT96"),
                CompanyName = "Organic Goods Co.",
                ContactName = "Alice Johnson",
                TaxNumber = "112233445",
                Phone = "+1122334455",
                Address = "789 Green Lane, City C",
            }
        );
    }
}

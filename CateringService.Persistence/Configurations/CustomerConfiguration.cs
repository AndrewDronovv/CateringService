using CateringService.Domain.Entities;
using CateringService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CateringService.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(d => d.Id)
           .HasColumnName("CustomerId")
           .IsRequired()
           .HasMaxLength(26)
           .HasConversion(
               id => id.ToString(),
               id => Ulid.Parse(id)
           );

        builder.Property(c => c.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.CustomerType)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<CustomerType>());

        builder.Property(c => c.CompanyName)
            .HasMaxLength(200);

        builder.Property(c => c.TaxNumber)
            .HasMaxLength(12);

        builder.HasData
        (
            new Customer
            {
                Id = Ulid.Parse("01H5QJ37V03WH5TXE2N1AW3JF9"),
                FullName = "John Doe",
                Phone = "+1-555-0123",
                CustomerType = CustomerType.Individual,
                CompanyName = null,
                TaxNumber = null
            },
            new Customer
            {
                Id = Ulid.Parse("01H5QJ38KGWM2N56TFH99WQZ03"),
                FullName = "Jane Smith",
                Phone = "+1-555-0456",
                CustomerType = CustomerType.Individual,
                CompanyName = null,
                TaxNumber = null
            },
            new Customer
            {
                Id = Ulid.Parse("01H5QJ391M8PVG6ZWPK4GTN0D8"),
                FullName = "Corporate Client",
                Phone = "+1-555-0789",
                CustomerType = CustomerType.Corporate,
                CompanyName = "ACME Inc.",
                TaxNumber = "1234567890"
            }
        );
    }
}

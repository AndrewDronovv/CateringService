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

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.ContactInfo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.PaymentType)
            .HasConversion(new EnumToStringConverter<PaymentType>())
            .IsRequired();

        builder.HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData
        (
            new Customer
            {
                Id = Ulid.Parse("01H5QJ37V03WH5TXE2N1AW3JF9"),
                Name = "John Doe",
                ContactInfo = "john.doe@example.com",
                PaymentType = PaymentType.CreditCard
            },
            new Customer
            {
                Id = Ulid.Parse("01H5QJ38KGWM2N56TFH99WQZ03"),
                Name = "Jane Smith",
                ContactInfo = "jane.smith@domain.com",
                PaymentType = PaymentType.PayPal

            },
            new Customer
            {
                Id = Ulid.Parse("01H5QJ391M8PVG6ZWPK4GTN0D8"),
                Name = "Corporate Client",
                ContactInfo = "contact@corporate.com",
                PaymentType = PaymentType.Cash
            }
        );
    }
}

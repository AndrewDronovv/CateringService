using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("CustomerId")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.ContactInfo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.PaymentType)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Тип оплаты, например, Кредитная карта, PayPal, Наличные");

        builder.HasMany(c => c.Orders)
            .WithOne(o => o.Customer) 
            .HasForeignKey(o => o.CustomerId) 
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData
        (
            new Customer
            {
                Id = 1,
                Name = "John Doe",
                ContactInfo = "john.doe@example.com",
                PaymentType = "Credit Card"
            },
            new Customer
            {
                Id = 2,
                Name = "Jane Smith",
                ContactInfo = "jane.smith@domain.com",
                PaymentType = "PayPal"
            },
            new Customer
            {
                Id = 3,
                Name = "Corporate Client",
                ContactInfo = "contact@corporate.com",
                PaymentType = "Invoice"
            }
        );
    }
}

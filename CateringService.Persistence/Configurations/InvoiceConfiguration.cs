using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasColumnName("InvoiceId")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(i => i.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.DateIssued)
            .IsRequired();

        builder.Property(i => i.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(i => i.Supplier)
            .WithMany(s => s.Invoices)
            .HasForeignKey(i => i.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.Broker)
            .WithMany(b => b.Invoices)
            .HasForeignKey(i => i.BrokerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData
        (
            new Invoice
            {
                Id = 1,
                Amount = 500.00m,
                DateIssued = new DateTime(2025, 4, 10),
                Status = "Paid",
                SupplierId = 1,
                BrokerId = 1
            },
            new Invoice
            {
                Id = 2,
                Amount = 1500.50m,
                DateIssued = new DateTime(2025, 4, 12),
                Status = "Unpaid",
                SupplierId = 2,
                BrokerId = 2
            },
            new Invoice
            {
                Id = 3,
                Amount = 800.75m,
                DateIssued = new DateTime(2025, 4, 14),
                Status = "Pending",
                SupplierId = 3,
                BrokerId = 3
            }
        );
    }
}

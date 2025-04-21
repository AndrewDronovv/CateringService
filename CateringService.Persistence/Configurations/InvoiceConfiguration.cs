using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(i => i.Id);

        builder.Property(d => d.Id)
            .HasColumnName("InvoiceId")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

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
                Id = Ulid.Parse("01H5QJ3CZ4FBZAMT62XXYY24FZ"),
                Amount = 500.00m,
                DateIssued = new DateTime(2025, 4, 10),
                Status = "Paid",
                SupplierId = Ulid.Parse("01H5QJ6PTMVRFZT58GQX902JC4"),
                BrokerId = Ulid.Parse("01H5QJ35QJ64MC1BTD5NRQ34R7")
            },
            new Invoice
            {
                Id = Ulid.Parse("01H5QJ3D5T7JV9B1VQF6BRFV4P"),
                Amount = 1500.50m,
                DateIssued = new DateTime(2025, 4, 12),
                Status = "Unpaid",
                SupplierId = Ulid.Parse("01H5QJ6PVB8FYN4QXMR3T7JC9A"),
                BrokerId = Ulid.Parse("01H5QJ36N1WHX5KDPQQGTVPVHC")
            },
            new Invoice
            {
                Id = Ulid.Parse("01H5QJ3DF6RQG96Q3VK7JBY58N"),
                Amount = 800.75m,
                DateIssued = new DateTime(2025, 4, 14),
                Status = "Pending",
                SupplierId = Ulid.Parse("01H5QJ6PX4FTQY8KZVW9JMBT96"),
                BrokerId = Ulid.Parse("01H5QJ379P7NZR1X03XW0GM7MA")
            }
        );
    }
}

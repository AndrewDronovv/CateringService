using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class BrokerConfiguration : IEntityTypeConfiguration<Broker>
{
    public void Configure(EntityTypeBuilder<Broker> builder)
    {
        builder.ToTable("Brokers");

        builder.HasKey(b => b.Id);

        builder.Property(d => d.Id)
            .HasColumnName("BrokerId")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.ContactInfo)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(b => b.Invoices)
            .WithOne(i => i.Broker)
            .HasForeignKey(i => i.BrokerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(b => b.Reports)
            .WithOne(r => r.Broker)
            .HasForeignKey(r => r.BrokerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData
        (
            new Broker
            {
                Id = Ulid.Parse("01H5QJ35QJ64MC1BTD5NRQ34R7"),
                Name = "Gourmet Catering",
                ContactInfo = "info@gourmetcatering.com",
            },
            new Broker
            {
                Id = Ulid.Parse("01H5QJ36N1WHX5KDPQQGTVPVHC"),
                Name = "Healthy Kitchen",
                ContactInfo = "contact@healthykitchen.com",
            },
            new Broker
            {
                Id = Ulid.Parse("01H5QJ379P7NZR1X03XW0GM7MA"),
                Name = "Event Planners Co.",
                ContactInfo = "support@eventplanners.com",
            }
        );
    }
}
using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CateringService.Persistence.Configurations;

public class BrokerConfiguration : IEntityTypeConfiguration<Broker>
{
    public void Configure(EntityTypeBuilder<Broker> builder)
    {
        builder.ToTable("Brokers");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("BrokerId")
            .ValueGeneratedOnAdd()
            .IsRequired();

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
                Id = 1,
                Name = "Gourmet Catering",
                ContactInfo = "info@gourmetcatering.com",
            },
            new Broker
            {
                Id = 2,
                Name = "Healthy Kitchen",
                ContactInfo = "contact@healthykitchen.com",
            },
            new Broker
            {
                Id = 3,
                Name = "Event Planners Co.",
                ContactInfo = "support@eventplanners.com",
            }
        );
    }
}
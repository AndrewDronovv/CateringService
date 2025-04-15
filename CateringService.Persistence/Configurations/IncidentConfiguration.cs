using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public class IncidentConfiguration : IEntityTypeConfiguration<Incident>
{
    public void Configure(EntityTypeBuilder<Incident> builder)
    {
        builder.ToTable("Incidents");

        builder.HasKey(i => i.Id);

        builder.Property(dp => dp.Id)
            .HasColumnName("IncidentId")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(i => i.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(i => i.Date)
            .IsRequired();

        builder.Property(i => i.Resolution)
            .HasMaxLength(500);

        builder.HasOne(i => i.Delivery)
            .WithMany(d => d.Incidents)
            .HasForeignKey(i => i.DeliveryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData
        (
            new Incident
            {
                Id = 1,
                Description = "Late delivery due to traffic jam",
                Date = new DateTime(2025, 4, 14),
                Resolution = "Customer notified and accepted delay",
                DeliveryId = 1
            },
            new Incident
            {
                Id = 2,
                Description = "Damaged package during delivery",
                Date = new DateTime(2025, 4, 15),
                Resolution = "Replacement item sent to customer",
                DeliveryId = 2
            },
            new Incident
            {
                Id = 3,
                Description = "Wrong address provided by customer",
                Date = new DateTime(2025, 4, 16),
                Resolution = "Correct address obtained and delivery rescheduled",
                DeliveryId = 3
            }
        );
    }
}

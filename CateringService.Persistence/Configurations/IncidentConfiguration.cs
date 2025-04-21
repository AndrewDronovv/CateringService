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

        builder.Property(d => d.Id)
            .HasColumnName("IncidentId")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

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
                Id = Ulid.Parse("01H5QJ3BTSX3JJ3F6DTQVFX86P"),
                Description = "Late delivery due to traffic jam",
                Date = new DateTime(2025, 4, 14),
                Resolution = "Customer notified and accepted delay",
                DeliveryId = Ulid.Parse("01H5QJ399WTKN11Z9FMB02WT62")
            },
            new Incident
            {
                Id = Ulid.Parse("01H5QJ3CB21J8GEPKGXZ80WRQ9"),
                Description = "Damaged package during delivery",
                Date = new DateTime(2025, 4, 15),
                Resolution = "Replacement item sent to customer",
                DeliveryId = Ulid.Parse("01H5QJ39VRZ2AN3YC94PM5FMPA")
            },
            new Incident
            {
                Id = Ulid.Parse("01H5QJ3CC0PF6XRTA21DW3QPEK"),
                Description = "Wrong address provided by customer",
                Date = new DateTime(2025, 4, 16),
                Resolution = "Correct address obtained and delivery rescheduled",
                DeliveryId = Ulid.Parse("01H5QJ3A8D7V2GPF2K4K3WH5C4")
            }
        );
    }
}

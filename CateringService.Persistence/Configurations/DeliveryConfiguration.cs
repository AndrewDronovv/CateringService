using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.ToTable("Deliveries");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
           .HasColumnName("DeliveryId")
           .IsRequired()
           .HasMaxLength(26)
           .HasConversion(
               id => id.ToString(),
               id => Ulid.Parse(id)
           );

        builder.Property(d => d.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(d => d.DeliveryPerson)
            .WithMany(dp => dp.Deliveries)
            .HasForeignKey(d => d.DeliveryPersonId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(d => d.Incidents)
            .WithOne(i => i.Delivery)
            .HasForeignKey(i => i.DeliveryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData
        (
            new Delivery
            {
                Id = Ulid.Parse("01H5QJ399WTKN11Z9FMB02WT62"),
                Status = "In Progress",
                DeliveryPersonId = Ulid.Parse("01H5QJ3AFV0T3ZQBGP19HK2K5V"),
            },
            new Delivery
            {
                Id = Ulid.Parse("01H5QJ39VRZ2AN3YC94PM5FMPA"),
                Status = "Completed",
                DeliveryPersonId = Ulid.Parse("01H5QJ3BBCEKJ7MYNVK302XRYF")
            },
            new Delivery
            {
                Id = Ulid.Parse("01H5QJ3A8D7V2GPF2K4K3WH5C4"),
                Status = "Delayed",
                DeliveryPersonId = Ulid.Parse("01H5QJ3BHR2FAYVZWNAD0XJJYE")
            }
        );
    }
}

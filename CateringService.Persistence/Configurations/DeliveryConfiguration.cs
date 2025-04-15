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
            .ValueGeneratedOnAdd()
            .IsRequired();

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
                Id = 1,
                Status = "In Progress",
                DeliveryPersonId = 1
            },
            new Delivery
            {
                Id = 2,
                Status = "Completed",
                DeliveryPersonId = 2
            },
            new Delivery
            {
                Id = 3,
                Status = "Delayed",
                DeliveryPersonId = 3
            }
        );
    }
}

using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public class DeliveryPersonConfiguration : IEntityTypeConfiguration<DeliveryPerson>
{
    public void Configure(EntityTypeBuilder<DeliveryPerson> builder)
    {
        builder.ToTable("DeliveryPersons");

        builder.HasKey(dp => dp.Id);

        builder.Property(d => d.Id)
           .HasColumnName("DeliveryPersonId")
           .IsRequired()
           .HasMaxLength(26)
           .HasConversion(
               id => id.ToString(),
               id => Ulid.Parse(id)
           );

        builder.Property(dp => dp.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(dp => dp.ContactInfo)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(dp => dp.Deliveries)
            .WithOne(d => d.DeliveryPerson)
            .HasForeignKey(d => d.DeliveryPersonId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData
        (
            new DeliveryPerson
            {
                Id = Ulid.Parse("01H5QJ3AFV0T3ZQBGP19HK2K5V"),
                Name = "Alex Johnson",
                ContactInfo = "alex.johnson@delivery.com"
            },
            new DeliveryPerson
            {
                Id = Ulid.Parse("01H5QJ3BBCEKJ7MYNVK302XRYF"),
                Name = "Maria Gonzalez",
                ContactInfo = "maria.gonzalez@delivery.com"
            },
            new DeliveryPerson
            {
                Id = Ulid.Parse("01H5QJ3BHR2FAYVZWNAD0XJJYE"),
                Name = "William Smith",
                ContactInfo = "william.smith@delivery.com"
            }
        );
    }
}
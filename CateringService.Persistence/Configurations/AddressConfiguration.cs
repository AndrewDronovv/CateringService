using CateringService.Domain.Entities.Approved;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");

        builder.HasKey(a => a.Id);

        builder.HasIndex(a => new { a.City, a.StreetAndBuilding })
            .HasMethod("GIN")
            .IsTsVectorExpressionIndex("english");

        builder.Property(a => a.Id)
            .HasColumnName("Id")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id));

        builder.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(a => a.StreetAndBuilding)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(a => a.Zip)
            .IsRequired()
            .HasMaxLength(6)
            .HasColumnType("char(6)");

        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(a => a.Region)
            .HasMaxLength(64);

        builder.Property(a => a.Comment)
            .HasMaxLength(256);

        builder.Property(a => a.Description)
            .HasMaxLength(512);

        builder.Property(a => a.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(a => a.UpdatedAt)
            .HasDefaultValue(null)
            .ValueGeneratedOnUpdate();

        builder.HasIndex(a => a.TenantId);

        builder.HasIndex(a => a.Zip)
            .IsUnique();
    }
}
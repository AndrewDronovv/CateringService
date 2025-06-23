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
            .HasDefaultValueSql("NULL")
            .ValueGeneratedOnUpdate();

        builder.HasIndex(a => a.TenantId);

        builder.HasIndex(a => a.Zip)
            .IsUnique();

        builder.HasData
        (
            new Address
            {
                Id = Ulid.Parse("01H5QJ8KTMVRFZT58GQX902JD1"),
                TenantId = Ulid.Parse("01H5PY6RF4WKFCR9VCMY2QNFGP"),
                Country = "USA",
                StreetAndBuilding = "123 Main St",
                Zip = "100001",
                City = "New York",
                Region = "NY",
                Comment = "Office address",
                Description = "Main headquarters",
                CreatedAt = new DateTime(2025, 04, 21, 08, 30, 0),
            },
            new Address
            {
                Id = Ulid.Parse("01H5QJ8RTMVRFZT58GQX902JD2"),
                TenantId = Ulid.Parse("01H5QJ6PVB8FYN4QXMR3T7JC9A"),
                Country = "Germany",
                StreetAndBuilding = "45 Berliner Str.",
                Zip = "200002",
                City = "Berlin",
                Region = "Berlin",
                Comment = "Warehouse",
                Description = "Storage facility",
                CreatedAt = new DateTime(2025, 04, 22, 12, 15, 0),
            },
            new Address
            {
                Id = Ulid.Parse("01H5QJ8UTMVRFZT58GQX902JD3"),
                TenantId = Ulid.Parse("01H5QJ7XPLKTYZ9QW8VRCMND5B"),
                Country = "Japan",
                StreetAndBuilding = "7-2 Shibuya",
                Zip = "300003",
                City = "Tokyo",
                Region = "Kanto",
                Comment = "Retail store",
                Description = "Flagship location",
                CreatedAt = new DateTime(2025, 04, 23, 09, 00, 0),
            }
        );
    }
}
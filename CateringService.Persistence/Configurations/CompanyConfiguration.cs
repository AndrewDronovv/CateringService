using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

        builder.Property(c => c.TenantId)
            .IsRequired();

        builder.Property(c => c.AddressId)
            .IsRequired();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.TaxNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(c => c.TaxNumber)
            .IsUnique();

        builder.HasIndex(c => c.TenantId);

        builder.Property(c => c.IsBlocked)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.Email)
            .HasMaxLength(100);

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasDefaultValueSql("NULL")
            .ValueGeneratedOnUpdate();

        builder.HasOne(c => c.Tenant)
            .WithMany(t => t.Companies)
            .HasForeignKey(c => c.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Address)
            .WithMany(a => a.Companies)
            .HasForeignKey(c => c.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Company
            {
                Id = Ulid.Parse("01HY5K3D15E8BC6X9J9ZKBPNSM"),
                Name = "TechSpace Ltd.",
                TenantId = Ulid.Parse("01H5PY6RF4WKFCR9VCMY2QNFGP"),
                AddressId = Ulid.Parse("01H5QJ8KTMVRFZT58GQX902JD1"),
                TaxNumber = "1234567890",
                Phone = "+1-555-1234",
                Email = "info@techspace.com",
                IsBlocked = false,
            },
            new Company
            {
                Id = Ulid.Parse("01HY5K3NCA4D8RYYWRZZ1RZD1X"),
                Name = "Greencore Solutions",
                TenantId = Ulid.Parse("01H5QJ6PVB8FYN4QXMR3T7JC9A"),
                AddressId = Ulid.Parse("01H5QJ8RTMVRFZT58GQX902JD2"),
                TaxNumber = "9876543210",
                Phone = "+1-555-6789",
                Email = "contact@greencore.io",
                IsBlocked = false,
            },
            new Company
            {
                Id = Ulid.Parse("01HY5K3SH4XNFQ6MTFD1EZRAZB"),
                Name = "NovaIndustries Inc.",
                TenantId = Ulid.Parse("01H5QJ7XQZKTYZ9QW8VRCMND5B"),
                AddressId = Ulid.Parse("01H5QJ9ZTMVRFZT58GQX902JD3"),
                TaxNumber = "1122334455",
                Phone = "+1-555-4321",
                Email = "hello@novainc.com",
                IsBlocked = false,
            }
        );
    }
}
using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("TenantId")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.IsActive);

        builder.Property(t => t.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasData(
            new Tenant
            {
                Id = Ulid.Parse("01H5PY6RF4WKFCR9VCMY2QNFGP"),
                Name = "First tenant",
                IsActive = true,
                CreatedAt = new DateTime(2025, 04, 20, 10, 0, 0)
            },
            new Tenant
            {
                Id = Ulid.Parse("01H5QJ6PVB8FYN4QXMR3T7JC9A"),
                Name = "Second tenant",
                IsActive = true,
                CreatedAt = new DateTime(2025, 04, 21, 12, 30, 0)
            },
            new Tenant
            {
                Id = Ulid.Parse("01H5QJ7XPLKTYZ9QW8VRCMND5B"),
                Name = "Third tenant",
                IsActive = true,
                CreatedAt = new DateTime(2025, 04, 22, 14, 15, 0)
            }
        );
    }
}
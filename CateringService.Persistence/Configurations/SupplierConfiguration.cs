using CateringService.Domain.Entities.Approved;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.Property(s => s.Position)
            .HasMaxLength(100);

        builder.Property(s => s.CompanyId)
            .IsRequired()
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );
    }
}
using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CateringService.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(c => c.TaxNumber)
            .IsRequired();

        builder.Property(c => c.CompanyId)
            .IsRequired(false)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id));

        builder.Property(c => c.AddressId)
            .IsRequired(false)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id));
    }
}
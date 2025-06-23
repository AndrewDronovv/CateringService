using CateringService.Domain.Entities.Approved;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

        builder.HasData(
            new Company
            {
                Id = Ulid.Parse("01HY5K3D15E8BC6X9J9ZKBPNSM"),
                Name = "Company one",
            },
            new Company
            {
                Id = Ulid.Parse("01HY5K3NCA4D8RYYWRZZ1RZD1X"),
                Name = "Company two",
            },
            new Company
            {
                Id = Ulid.Parse("01HY5K3SH4XNFQ6MTFD1EZRAZB"),
                Name = "Company three",
            }
        );
    }
}
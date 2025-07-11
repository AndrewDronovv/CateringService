﻿using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(c => c.TaxNumber)
            .IsRequired();

        builder.Property(c => c.CompanyId)
            .IsRequired()
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id));

        builder.Property(c => c.AddressId)
            .IsRequired(false)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id));

        builder.HasIndex(c => c.CompanyId)
            .IsUnique();

        builder.HasIndex(c => c.AddressId)
            .IsUnique();
    }
}
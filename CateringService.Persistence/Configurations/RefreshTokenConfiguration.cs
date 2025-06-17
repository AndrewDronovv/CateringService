using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("TokenId")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

        builder.Property(t => t.Token)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Expires)
            .IsRequired();

        builder.Property(t => t.Created)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(t => t.Revoked);

        builder.Property(t => t.UserId)
            .IsRequired()
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );
    }
}
using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("Id")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(u => u.MiddleName)
            .HasMaxLength(128);

        builder.Property(u => u.Email)
            .HasMaxLength(100);

        builder.Property(u => u.Phone)
            .HasMaxLength(20);

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.IsBlocked)
            .HasDefaultValue(false);

        builder.Property(u => u.BlockReason)
            .HasMaxLength(512);

        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(u => u.UpdatedAt);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasOne(u => u.Tenant)
            .WithMany(t => t.Users)
            .HasForeignKey(u => u.TenantId);

        builder
            .HasDiscriminator<UserType>("UserType")
            .HasValue<User>(UserType.User)
            .HasValue<Customer>(UserType.Customer)
            .HasValue<Supplier>(UserType.Supplier)
            .HasValue<Broker>(UserType.Broker);

        builder
            .Property(nameof(UserType))
            .HasConversion<string>();
    }
}
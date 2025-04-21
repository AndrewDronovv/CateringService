using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.ToTable("Reports");

        builder.HasKey(r => r.Id);

        builder.Property(d => d.Id)
            .HasColumnName("ReportId")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

        builder.Property(r => r.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Details)
            .HasMaxLength(1000);

        builder.Property(r => r.GeneratedDate)
            .IsRequired();

        builder.HasOne(r => r.Broker)
            .WithMany(b => b.Reports)
            .HasForeignKey(r => r.BrokerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData
        (
            new Report
            {
                Id = Ulid.Parse("01H5QJ6PJXP3KN3ZMCXGTFY8P9"),
                Type = "Performance",
                Details = "Detailed performance report for Q1.",
                GeneratedDate = new DateTime(2025, 4, 1),
                BrokerId = Ulid.Parse("01H5QJ35QJ64MC1BTD5NRQ34R7")
            },
            new Report
            {
                Id = Ulid.Parse("01H5QJ6PMZ48BVTCJMK30RW9J6"),
                Type = "Compliance",
                Details = "Compliance report for catering regulations.",
                GeneratedDate = new DateTime(2025, 4, 5),
                BrokerId = Ulid.Parse("01H5QJ36N1WHX5KDPQQGTVPVHC")
            },
            new Report
            {
                Id = Ulid.Parse("01H5QJ6PRJAXFV54N82M3TQXJY"),
                Type = "Financial",
                Details = "Comprehensive financial analysis for last quarter.",
                GeneratedDate = new DateTime(2025, 4, 10),
                BrokerId = Ulid.Parse("01H5QJ379P7NZR1X03XW0GM7MA")
            }
        );
    }
}

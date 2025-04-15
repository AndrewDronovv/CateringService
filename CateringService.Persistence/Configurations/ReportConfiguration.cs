using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.ToTable("Reports");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("ReportId")
            .ValueGeneratedOnAdd()
            .IsRequired();

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
                Id = 1,
                Type = "Performance",
                Details = "Detailed performance report for Q1.",
                GeneratedDate = new DateTime(2025, 4, 1),
                BrokerId = 1
            },
            new Report
            {
                Id = 2,
                Type = "Compliance",
                Details = "Compliance report for catering regulations.",
                GeneratedDate = new DateTime(2025, 4, 5),
                BrokerId = 2
            },
            new Report
            {
                Id = 3,
                Type = "Financial",
                Details = "Comprehensive financial analysis for last quarter.",
                GeneratedDate = new DateTime(2025, 4, 10),
                BrokerId = 3
            }
        );
    }
}

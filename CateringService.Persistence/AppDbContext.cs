using CateringService.Domain.Entities;
using CateringService.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Broker> Brokers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BrokerConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    }
}

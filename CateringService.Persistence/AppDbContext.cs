using CateringService.Domain.Entities;
using CateringService.Domain.Entities.Approved;
using CateringService.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<MenuCategory> MenuCategories { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Address> Addresses { get; set; }

    public DbSet<Broker> Brokers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<DeliveryPerson> DeliveryPersons { get; set; }
    public DbSet<Incident> Incidents { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BrokerConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new DeliveryConfiguration());
        modelBuilder.ApplyConfiguration(new DeliveryPersonConfiguration());
        modelBuilder.ApplyConfiguration(new DishConfiguration());
        modelBuilder.ApplyConfiguration(new IncidentConfiguration());
        modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
        modelBuilder.ApplyConfiguration(new MenuCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        modelBuilder.ApplyConfiguration(new PromotionConfigurtaion());
        modelBuilder.ApplyConfiguration(new ReportConfiguration());
        modelBuilder.ApplyConfiguration(new SupplierConfiguration());
        modelBuilder.ApplyConfiguration(new TenantConfiguration());
        modelBuilder.ApplyConfiguration(new AddressConfiguration());

    }
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder
    //        .UseSeeding((context, _) =>
    //        {
    //            var menuCategory = context.Set<MenuCategory>().FirstOrDefault(mc => mc.Name == "Desserts");
    //            if (menuCategory == null)
    //            {
    //                context.Set<MenuCategory>().Add(new MenuCategory { Id = Ulid.NewUlid(), Name = "None", Description = "None" });
    //                context.SaveChanges();
    //            }
    //        })
    //        .UseAsyncSeeding(async (context, _, cancellationToken) =>
    //        {
    //            var menuCategory = await context
    //            .Set<MenuCategory>()
    //            .FirstOrDefaultAsync(mc => mc.Name == "Desserts", cancellationToken);
    //            if (menuCategory is null)
    //            {
    //                context.Set<MenuCategory>().Add(new MenuCategory { Id = Ulid.NewUlid(), Name = "None", Description = "None" });
    //                await context.SaveChangesAsync(cancellationToken);
    //            }
    //        });
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<DateTime>().HaveColumnType("timestamp without time zone");
        configurationBuilder.Properties<DateTime?>().HaveColumnType("timestamp with time zone");
    }
}
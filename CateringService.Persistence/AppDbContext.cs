using CateringService.Domain.Entities;
using CateringService.Domain.Enums;
using CateringService.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<MenuCategory> MenuCategories { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AddressConfiguration());
        modelBuilder.ApplyConfiguration(new BrokerConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new DishConfiguration());
        modelBuilder.ApplyConfiguration(new MenuCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new SupplierConfiguration());
        modelBuilder.ApplyConfiguration(new TenantConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        modelBuilder.Entity<Supplier>().HasData(
            new Supplier
            {
                Id = Ulid.Parse("01HY5Q0RPNMXCA2W6JXDMVVZ7B"),
                FirstName = "Irina",
                LastName = "Kulikova",
                MiddleName = "Alekseyevna",
                Email = "ikulikova@cateringservice.ru",
                Phone = "+7 (495) 123-45-67",
                PasswordHash = "hashed_password_here",
                IsBlocked = false,
                CreatedAt = new DateTime(2025, 04, 21, 12, 30, 0),
                TenantId = Ulid.Parse("01H5PY6RF4WKFCR9VCMY2QNFGP"),
                Position = "Supply Manager",
                CompanyId = Ulid.Parse("01HY5K3D15E8BC6X9J9ZKBPNSM")
            },
            new Supplier
            {
                Id = Ulid.Parse("01HY5Q0WRK6VFYHT9BA3H8RK3V"),
                FirstName = "Ivan",
                LastName = "Ivanov",
                MiddleName = "Ivanovich",
                Email = "ivanov@cateringservice.ru",
                Phone = "+7 (495) 155-55-67",
                PasswordHash = "new_hashed_password",
                IsBlocked = false,
                CreatedAt = new DateTime(2025, 04, 21, 12, 30, 0),
                TenantId = Ulid.Parse("01H5QJ6PVB8FYN4QXMR3T7JC9A"),
                Position = "Sales Manager",
                CompanyId = Ulid.Parse("01HY5K3NCA4D8RYYWRZZ1RZD1X")
            }
        );

        modelBuilder.Entity<Broker>().HasData(
            new Broker
            {
                Id = Ulid.Parse("01HY5Q13CZD9FXT78GR1XWA2XB"),
                FirstName = "Dmitry",
                LastName = "Sorokin",
                MiddleName = "Petrovich",
                Email = "dsorokin@brokeragepro.ru",
                Phone = "+7 (495) 987-65-43",
                PasswordHash = "hashed_secure_password",
                IsBlocked = false,
                BlockReason = null,
                CreatedAt = new DateTime(2025, 04, 21, 12, 30, 0),
                TenantId = Ulid.Parse("01H5QJ7XPLKTYZ9QW8VRCMND5B"),
                Role = BrokerRole.Accountant
            }
        );

        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                Id = Ulid.Parse("01HYZZZX7TS6AXK9R29X3PXJPX"),
                FirstName = "Olga",
                LastName = "Smirnova",
                MiddleName = "Ivanovna",
                Email = "osmirnova@cateringservice.ru",
                Phone = "+7 (495) 000-11-22",
                PasswordHash = "hashed_customer_password",
                IsBlocked = false,
                BlockReason = null,
                CreatedAt = new DateTime(2025, 04, 21, 12, 30, 0),
                TaxNumber = 123456789,
                CompanyId = Ulid.Parse("01HY5K3D15E8BC6X9J9ZKBPNSM"),
                TenantId = Ulid.Parse("01H5PY6RF4WKFCR9VCMY2QNFGP")
            }
        );
    }
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<DateTime>().HaveColumnType("timestamp without time zone");
        configurationBuilder.Properties<DateTime?>().HaveColumnType("timestamp with time zone");
    }
}
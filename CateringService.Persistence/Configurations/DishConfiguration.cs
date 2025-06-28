using CateringService.Domain.Entities.Approved;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public sealed class DishConfiguration : IEntityTypeConfiguration<Dish>
{
    public void Configure(EntityTypeBuilder<Dish> builder)
    {
        builder.ToTable("Dishes");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("Id")
            .IsRequired()
            .HasMaxLength(26)
            .HasConversion(
                id => id.ToString(),
                id => Ulid.Parse(id)
            );

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Description)
            .HasMaxLength(500);

        builder.Property(d => d.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(d => d.Ingredients)
            .HasMaxLength(500);

        builder.Property(d => d.Weight)
            .IsRequired();

        builder.Property(d => d.ImageUrl)
            .HasMaxLength(200);

        builder.Property(d => d.IsAvailable)
            .IsRequired();

        builder.Property(d => d.Allergens)
            .HasMaxLength(500);

        builder.Property(d => d.PortionSize)
            .HasMaxLength(150);

        builder.Property(d => d.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.HasOne(d => d.Supplier)
            .WithMany(s => s.Dishes)
            .HasForeignKey(d => d.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(d => d.MenuCategory)
            .WithMany(ms => ms.Dishes)
            .HasForeignKey(d => d.MenuCategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(d => d.Id);

        builder.HasData
        (
            new Dish
            {
                Id = Ulid.Parse("01GRQX9AYRHCA5Y5X3GPKPZ92P"),
                Name = "Grilled Chicken",
                Description = "Juicy grilled chicken with spices",
                Price = 12.99m,
                Ingredients = "Chicken, spices, olive oil",
                Weight = 250,
                ImageUrl = "/images/GrilledChicken.webp",
                IsAvailable = true,
                Allergens = "None",
                PortionSize = "Large",
                CreatedAt = new DateTime(2025, 04, 20, 10, 0, 0),
                MenuCategoryId = Ulid.Parse("01H5QJ3DHBM8J6AW04FKPJP5VV"),
                SupplierId = Ulid.Parse("01HY5Q0RPNMXCA2W6JXDMVVZ7B")
            },
            new Dish
            {
                Id = Ulid.Parse("01GRQX9AYRHCA5Y5X3GPKPZ93Q"),
                Name = "Vegetable Salad",
                Description = "Fresh seasonal vegetables with olive oil",
                Price = 8.50m,
                Ingredients = "Lettuce, tomatoes, cucumber, olive oil",
                Weight = 150,
                ImageUrl = "/images/VegetableSalad.jpg",
                IsAvailable = true,
                Allergens = "None",
                PortionSize = "Medium",
                CreatedAt = new DateTime(2025, 04, 20, 12, 0, 0),
                MenuCategoryId = Ulid.Parse("01H5QJ3DJ22VXVG28Q0RYMNQEY"),
                SupplierId = Ulid.Parse("01HY5Q0RPNMXCA2W6JXDMVVZ7B")
            },
            new Dish
            {
                Id = Ulid.Parse("01H5PY6RCAKEQ7VNK35P6XZ48Z"),
                Name = "Chocolate Cake",
                Description = "Rich and creamy chocolate cake",
                Price = 5.99m,
                Ingredients = "Chocolate, flour, sugar, eggs, butter",
                Weight = 300,
                ImageUrl = "/images/ChocolateCake.jpg",
                IsAvailable = false,
                Allergens = "Eggs, Milk",
                PortionSize = "Small",
                CreatedAt = new DateTime(2025, 04, 20, 14, 0, 0),
                MenuCategoryId = Ulid.Parse("01H5QJ3DR6R35WTKTPGFPJ89JC"),
                SupplierId = Ulid.Parse("01HY5Q0RPNMXCA2W6JXDMVVZ7B")
            }
        );
    }
}
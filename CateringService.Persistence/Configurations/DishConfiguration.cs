using CateringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CateringService.Persistence.Configurations;

public class DishConfiguration : IEntityTypeConfiguration<Dish>
{
    public void Configure(EntityTypeBuilder<Dish> builder)
    {
        builder.ToTable("Dishes");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("DishId")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Descritpion)
            .HasMaxLength(500);

        builder.Property(d => d.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(d => d.Ingredients)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(d => d.Weight)
            .IsRequired();

        builder.Property(d => d.Image)
            .HasMaxLength(200);

        builder.Property(d => d.AvailabilityStatus)
            .IsRequired();

        builder.HasOne(d => d.Supplier)
            .WithMany(s => s.Dishes)
            .HasForeignKey(d => d.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(d => d.MenuSection)
            .WithMany(ms => ms.Dishes)
            .HasForeignKey(d => d.MenuSectionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData
        (
            new Dish
            {
                Id = 1,
                Name = "Grilled Chicken",
                Descritpion = "Juicy grilled chicken with spices",
                Price = 12.99m,
                Ingredients = "Chicken, spices, olive oil",
                Weight = 250,
                Image = "grilled_chicken.jpg",
                AvailabilityStatus = true,
                SupplierId = 1,
                MenuSectionId = 1
            },
            new Dish
            {
                Id = 2,
                Name = "Vegetable Salad",
                Descritpion = "Fresh seasonal vegetables with olive oil",
                Price = 8.50m,
                Ingredients = "Lettuce, tomatoes, cucumber, olive oil",
                Weight = 150,
                Image = "veggie_salad.jpg",
                AvailabilityStatus = true,
                SupplierId = 2,
                MenuSectionId = 2
            },
            new Dish
            {
                Id = 3,
                Name = "Chocolate Cake",
                Descritpion = "Rich and creamy chocolate cake",
                Price = 5.99m,
                Ingredients = "Chocolate, flour, sugar, eggs, butter",
                Weight = 300,
                Image = "chocolate_cake.jpg",
                AvailabilityStatus = false,
                SupplierId = 3,
                MenuSectionId = 3
            }
        );
    }
}

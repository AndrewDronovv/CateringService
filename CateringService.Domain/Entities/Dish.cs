using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Dish : UlidEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Ingredients { get; set; } = string.Empty;
    public double Weight { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
    public string? Allergens { get; set; } = string.Empty;
    public string? PortionSize { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Ulid SupplierId { get; set; }
    public Supplier Supplier { get; set; }

    public Ulid MenuCategoryId { get; set; }
    public MenuCategory MenuCategory { get; set; }
}
using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Dish : UlidEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Ingredients { get; set; } = string.Empty;
    public double Weight { get; set; }
    public string Image { get; set; } = string.Empty;
    public bool AvailabilityStatus { get; set; }

    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; }
    public int MenuSectionId { get; set; }
    public MenuSection MenuSection { get; set; }
}
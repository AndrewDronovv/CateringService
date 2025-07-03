using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class MenuCategory : UlidEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public Ulid SupplierId { get; set; }
    public Supplier Supplier { get; set; }
    public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
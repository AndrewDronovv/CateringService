using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class MenuSection : Entity
{
    public string Name { get; set; } = string.Empty;
    
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; }
    public ICollection<Dish> Dishes { get; set; }
}

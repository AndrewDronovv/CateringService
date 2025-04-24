using CateringService.Domain.Entities;

namespace CateringService.Application.DataTransferObjects.MenuCategory;

public class MenuCategoryCreateDto
{
    public Ulid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Ulid SupplierId { get; set; }
    public Supplier Supplier { get; set; }
}
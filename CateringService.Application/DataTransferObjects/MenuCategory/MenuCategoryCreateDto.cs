namespace CateringService.Application.DataTransferObjects.MenuCategory;

public sealed class MenuCategoryCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Ulid SupplierId { get; set; }
}
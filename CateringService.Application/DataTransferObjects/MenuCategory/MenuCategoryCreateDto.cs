namespace CateringService.Application.DataTransferObjects.MenuCategory;

public class MenuCategoryCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Ulid SupplierId { get; set; }
}
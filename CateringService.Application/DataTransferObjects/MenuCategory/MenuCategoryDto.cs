namespace CateringService.Application.DataTransferObjects.MenuCategory;

public sealed class MenuCategoryDto
{
    public Ulid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
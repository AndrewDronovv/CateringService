namespace CateringService.Application.DataTransferObjects.MenuCategory;

public sealed class MenuCategoryUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
}
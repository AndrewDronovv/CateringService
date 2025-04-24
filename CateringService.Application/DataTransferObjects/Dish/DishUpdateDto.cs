namespace CateringService.Application.DataTransferObjects.Dish;

public sealed class DishUpdateDto
{
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
}
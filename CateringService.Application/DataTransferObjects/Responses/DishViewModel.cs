namespace CateringService.Application.DataTransferObjects.Responses;

public sealed class DishViewModel
{
    public Ulid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Ingredients { get; set; } = string.Empty;
    public double Weight { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
    public string? Allergens { get; set; }
    public string? PortionSize { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Ulid SupplierId { get; set; }
    public Ulid MenuCategoryId { get; set; }
    public string? Slug { get; set; }
    public string CreatedAtString => CreatedAt.ToString("f");
}
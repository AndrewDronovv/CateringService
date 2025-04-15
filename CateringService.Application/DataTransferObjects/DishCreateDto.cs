namespace CateringService.Application.DataTransferObjects;

public sealed class DishCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Descritpion { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Ingredients { get; set; } = string.Empty;
    public double Weight { get; set; }
    public string Image { get; set; } = string.Empty;
    public bool AvailabilityStatus { get; set; }
    public int SupplierId { get; set; }
    public int MenuSectionId { get; set; }
}
namespace CateringService.Application.DataTransferObjects.Requests;

public sealed class UpdateDishRequest
{
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
}
namespace CateringService.Application.DataTransferObjects.Responses;

public sealed class MenuCategoryViewModel
{
    public Ulid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
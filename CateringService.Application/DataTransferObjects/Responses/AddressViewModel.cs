namespace CateringService.Application.DataTransferObjects.Responses;

public sealed class AddressViewModel
{
    public Ulid Id { get; set; }
    public Ulid TenantId { get; set; }
    public string Country { get; set; } = string.Empty;
    public string StreetAndBuilding { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? Region { get; set; } = string.Empty;
    public string? Comment { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

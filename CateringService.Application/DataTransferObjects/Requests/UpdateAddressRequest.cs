using CateringService.Domain.Entities;

namespace CateringService.Application.DataTransferObjects.Requests;

public sealed class UpdateAddressRequest
{
    public string Country { get; set; } = string.Empty;
    public string StreetAndBuilding { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? Region { get; set; } = string.Empty;
    public string? Comment { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
}

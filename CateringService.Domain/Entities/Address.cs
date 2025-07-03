using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Address : UlidEntity
{
    public string Country { get; set; } = string.Empty;
    public string StreetAndBuilding { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? Region { get; set; } = string.Empty;
    public string? Comment { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Ulid TenantId { get; set; }
    public Tenant Tenant { get; set; }
    public ICollection<Company> Companies { get; set; } = new List<Company>();
}
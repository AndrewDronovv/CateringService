using CateringService.Domain.Common;
using Newtonsoft.Json;

namespace CateringService.Domain.Entities;

public sealed class Company : UlidEntity
{
    public Ulid TenantId { get; set; }
    public Tenant Tenant { get; set; }
    public string Name { get; set; }
    public string TaxNumber { get; set; }
    public Ulid AddressId { get; set; }
    public Address Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsBlocked { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime? UpdatedAt { get; set; }
}
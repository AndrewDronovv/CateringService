using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Tenant : UlidEntity
{
    public string Name { get; set; } = string.Empty;
    public bool? IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
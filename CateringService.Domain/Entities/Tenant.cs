using CateringService.Domain.Common;
using CateringService.Domain.Entities.Approved;

namespace CateringService.Domain.Entities;

public sealed class Tenant : UlidEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string? BlockReason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<User> Users { get; set; } = new List<User>();
}
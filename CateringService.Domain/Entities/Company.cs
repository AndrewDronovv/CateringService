using CateringService.Domain.Common;

namespace CateringService.Domain.Entities.Approved;

public sealed class Company : UlidEntity
{
    public string? Name { get; set; }
}
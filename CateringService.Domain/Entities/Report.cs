using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Report : UlidEntity
{
    public string Type { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public DateTime GeneratedDate { get; set; }

    public Ulid BrokerId { get; set; }
    public Broker Broker { get; set; }
}

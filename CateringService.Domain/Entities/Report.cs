using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Report : Entity
{
    public string Type { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public DateTime GeneratedDate { get; set; }

    public int BrokerId { get; set; }
    public Broker Broker { get; set; }
}

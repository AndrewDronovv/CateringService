using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Incident : Entity
{
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Resolution { get; set; } = string.Empty;

    public int DeliveryId { get; set; }
    public Delivery Delivery { get; set; }
}

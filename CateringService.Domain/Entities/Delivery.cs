using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Delivery : Entity
{
    public string Status { get; set; } = string.Empty;

    public int DeliveryPersonId { get; set; }
    public DeliveryPerson DeliveryPerson { get; set; }
    public ICollection<Incident> Incidents { get; set; } = [];
}
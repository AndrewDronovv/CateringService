using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Delivery : UlidEntity
{
    public string Status { get; set; } = string.Empty;

    public Ulid DeliveryPersonId { get; set; }
    public DeliveryPerson DeliveryPerson { get; set; }
    public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
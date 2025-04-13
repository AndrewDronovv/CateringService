using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class DeliveryPerson : Entity
{
    public string Name { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;

    public ICollection<Delivery> Deliveries { get; set; } = [];
}

using CateringService.Domain.Enums;

namespace CateringService.Domain.Entities.Approved;

public sealed class Broker : User
{
    public BrokerRole Role { get; set; }
}
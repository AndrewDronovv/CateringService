using CateringService.Domain.Enums;

namespace CateringService.Domain.Entities.Approved;

//Курьерская компания для доставки блюда покупателю
public sealed class Broker : User
{
    public BrokerRole Role { get; set; }
}
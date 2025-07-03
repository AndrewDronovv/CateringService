using CateringService.Domain.Enums;

namespace CateringService.Domain.Entities;

//Курьерская компания для доставки блюда покупателю
public sealed class Broker : User
{
    public BrokerRole Role { get; set; }
}
using CateringService.Domain.Common;
using CateringService.Domain.Enums;

namespace CateringService.Domain.Entities;

public sealed class Customer : Entity
{
    public string Name { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;
    public PaymentType PaymentType { get; set; }

    public ICollection<Order> Orders { get; set; } = [];
}
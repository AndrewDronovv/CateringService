using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Customer : Entity
{
    public string Name { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;
    public string PaymentType { get; set; } = string.Empty;

    public ICollection<Order> Orders { get; set; } = [];
}
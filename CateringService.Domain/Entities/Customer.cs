using CateringService.Domain.Common;
using CateringService.Domain.Enums;

namespace CateringService.Domain.Entities;

public sealed class Customer : UlidEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public CustomerType CustomerType { get; set; }
    public string? CompanyName { get; set; } = string.Empty;
    public string? TaxNumber { get; set; } = string.Empty;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
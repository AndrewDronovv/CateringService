using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Invoice : UlidEntity
{
    public decimal Amount { get; set; }
    public DateTime DateIssued { get; set; }
    public string Status { get; set; } = string.Empty;

    public Ulid SupplierId { get; set; }
    public Supplier Supplier { get; set; }
    public Ulid BrokerId { get; set; }
    public Broker Broker { get; set; }
}
using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Invoice : Entity
{
    public decimal Amount { get; set; }
    public DateTime DateIssued { get; set; }
    public string Status { get; set; } = string.Empty;

    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; }
    public int BrokerId { get; set; }
    public Broker Broker { get; set; }
}

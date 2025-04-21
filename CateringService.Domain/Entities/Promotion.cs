using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Promotion : UlidEntity
{
    public string Type { get; set; } = string.Empty;
    public decimal DiscountValue { get; set; }
    public string Condition { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public Ulid SupplierId { get; set; }
    public Supplier Supplier { get; set; }
}
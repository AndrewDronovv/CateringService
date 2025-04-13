using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Promotion : Entity
{
    public string Type { get; set; } = string.Empty;
    public decimal DiscountValue { get; set; }
    public string Condition { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; }
}
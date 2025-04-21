using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Order : UlidEntity
{
    public DateTime OrderDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;

    public Ulid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public Ulid SupplierId { get; set; }
    public Supplier Supplier { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = [];
    public Ulid DeliveryId { get; set; }
    public Delivery Delivery { get; set; }
}

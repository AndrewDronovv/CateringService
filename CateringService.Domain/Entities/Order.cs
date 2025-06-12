using CateringService.Domain.Common;
using CateringService.Domain.Entities.Approved;

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
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public Ulid DeliveryId { get; set; }
    public Delivery Delivery { get; set; }

    //public int AddressId { get; set; }
    //public Address Address { get; set; }
}
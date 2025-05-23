using CateringService.Domain.Common;
using CateringService.Domain.Entities.Approved;

namespace CateringService.Domain.Entities;

public sealed class OrderItem : UlidEntity
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public Ulid OrderId { get; set; }
    public Order Order { get; set; }
    public Ulid DishId { get; set; }
    public Dish Dish { get; set; }
}
using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class OrderItem : Entity
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; }
    //public int DishId { get; set; }
    //public Dish Dish { get; set; }
}
using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Supplier : UlidEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int WorkingHours { get; set; }
    public bool IsActive { get; set; }

    public ICollection<Dish> Dishes { get; set; } = [];
    public ICollection<MenuCategory> MenuSections { get; set; } = [];
    public ICollection<Promotion> Promotions { get; set; } = [];
    public ICollection<Invoice> Invoices { get; set; } = [];
    public ICollection<Order> Orders { get; set; } = [];
}
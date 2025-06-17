using CateringService.Domain.Common;
using CateringService.Domain.Entities.Approved;

namespace CateringService.Domain.Entities;

public sealed class Supplier : UlidEntity
{
    //public string? Position { get; set; }
    //public Ulid CompanyId { get; set; }
    //public Company Company { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    public ICollection<MenuCategory> MenuCategories { get; set; } = new List<MenuCategory>();
    public ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
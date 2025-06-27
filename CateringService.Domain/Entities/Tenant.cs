using CateringService.Domain.Common;
using CateringService.Domain.Entities.Approved;

namespace CateringService.Domain.Entities;

// Арендатор, каждый покупатель имеет свой уникальный Id арендатора,
// компаний у одного арендатора может быть несколько, здесь 
// арендатор будет выступать как Id для всех подкомпаний, которыми он владеет
public sealed class Tenant : UlidEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string? BlockReason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Company> Companies { get; set; } = new List<Company>();
}
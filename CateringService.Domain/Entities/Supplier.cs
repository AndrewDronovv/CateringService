namespace CateringService.Domain.Entities;

//Компания которая занимается приготовлением блюда
public sealed class Supplier : User
{
    public string? Position { get; set; }
    public Ulid CompanyId { get; set; }
    public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
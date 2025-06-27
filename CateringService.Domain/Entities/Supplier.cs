namespace CateringService.Domain.Entities.Approved;

//Компания которая занимается приготовлением блюда
public sealed class Supplier : User
{
    public string? Position { get; set; }
    public Ulid CompanyId { get; set; }
}
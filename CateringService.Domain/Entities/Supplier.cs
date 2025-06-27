namespace CateringService.Domain.Entities.Approved;

public sealed class Supplier : User
{
    public string? Position { get; set; }
    public Ulid CompanyId { get; set; }
}
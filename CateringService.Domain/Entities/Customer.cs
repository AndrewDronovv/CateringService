namespace CateringService.Domain.Entities.Approved;

public sealed class Customer : User
{
    public int TaxNumber { get; set; }
    public Ulid? CompanyId { get; set; }
    public Ulid? AddressId { get; set; }
}
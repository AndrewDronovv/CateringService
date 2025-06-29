namespace CateringService.Application.DataTransferObjects.Requests;

public sealed class UpdateCompanyRequest
{
    public Ulid Id { get; set; }
    public string Name { get; set; }
    public string TaxNumber { get; set; }
    public Ulid AddressId { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
}
namespace CateringService.Application.DataTransferObjects.Responses;

public sealed class CompanyViewModel
{
    public Ulid Id { get; set; }
    public Ulid TenantId { get; set; }
    public string Name { get; set; }
    public string TaxNumber { get; set; }
    public Ulid AddressId { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsBlocked { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
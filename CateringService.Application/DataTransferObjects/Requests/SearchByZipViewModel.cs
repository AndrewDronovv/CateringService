namespace CateringService.Application.DataTransferObjects.Requests;

public sealed class SearchByZipViewModel
{
    public Ulid? TenantId { get; set; }
    public string Zip { get; set; } = string.Empty;
}

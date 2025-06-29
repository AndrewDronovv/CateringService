namespace CateringService.Application.DataTransferObjects.Requests;

public sealed class GetCompaniesRequest
{
    public Ulid? TenantId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
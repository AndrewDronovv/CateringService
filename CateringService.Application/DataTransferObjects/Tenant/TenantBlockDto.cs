namespace CateringService.Application.DataTransferObjects.Tenants;

public sealed class TenantBlockDto
{
    public Ulid Id { get; set; }
    public string? BlockReason { get; set; } = string.Empty;
}

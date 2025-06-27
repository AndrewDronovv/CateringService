namespace CateringService.Application.DataTransferObjects.Requests;

public sealed class BlockTenantRequest
{
    public Ulid Id { get; set; }
    public string? BlockReason { get; set; } = string.Empty;
}
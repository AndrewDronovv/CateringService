namespace CateringService.Application.DataTransferObjects.Tenants;

public sealed class TenantCreateDto
{
    public string Name { get; set; } = string.Empty;
    public bool? IsActive { get; set; } = true;
}
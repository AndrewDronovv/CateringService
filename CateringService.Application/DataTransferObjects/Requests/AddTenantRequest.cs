namespace CateringService.Application.DataTransferObjects.Requests;

public sealed class AddTenantRequest
{
    public string Name { get; set; } = string.Empty;
    public bool? IsActive { get; set; } = true;
}
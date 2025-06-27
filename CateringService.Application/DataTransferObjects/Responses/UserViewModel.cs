using CateringService.Domain.Entities;

namespace CateringService.Application.DataTransferObjects.Responses;

public sealed class UserViewModel
{
    public Ulid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsBlocked { get; set; } = false;
    public string? BlockReason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Ulid TenantId { get; set; }
    public Tenant Tenant { get; set; }
}

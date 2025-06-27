using CateringService.Domain.Enums;

namespace CateringService.Application.DataTransferObjects.Requests;

public sealed class AddUserRequest
{
    public string UserType { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Ulid TenantId { get; set; }

    public BrokerRole? Role { get; set; }

    public int? TaxNumber { get; set; }
    public Ulid? CompanyId { get; set; }
    public Ulid? AddressId { get; set; }
    public string? Position { get; set; }
}
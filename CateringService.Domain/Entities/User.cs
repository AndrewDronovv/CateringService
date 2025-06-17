using CateringService.Domain.Common;
using CateringService.Domain.Enums;

namespace CateringService.Domain.Entities;

public sealed class User : UlidEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsBlocked { get; set; } = false;
    public Role Role { get; set; }
    public string? BlockReason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public Ulid TenantId { get; set; }
    public Tenant Tenant { get; set; }

    //public string Login { get; set; }
    //public string Password { get; set; }
    //public string? Email { get; set; }
    //public string? FirstName { get; set; }
    //public string? LastName { get; set; }
    //public string PasswordHash { get; set; } = string.Empty;
    //public bool IsBlocked { get; set; } = false;
    //public DateTime CreatedAt { get; set; } = DateTime.Now;
    //public ICollection<RefreshToken> RefreshTokens { get; set; }
}
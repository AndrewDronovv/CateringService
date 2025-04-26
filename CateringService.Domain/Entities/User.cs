using CateringService.Domain.Common;
using CateringService.Domain.Enums;

namespace CateringService.Domain.Entities;

public sealed class User : UlidEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; }
    public bool IsBlocked { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

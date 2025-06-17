using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class RefreshToken : UlidEntity
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;
    public Ulid UserId { get; set; }
    public User User { get; set; }
}
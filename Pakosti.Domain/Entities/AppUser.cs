using Microsoft.AspNetCore.Identity;

namespace Pakosti.Domain.Entities;

public class AppUser : IdentityUser<Guid>
{
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string? RefreshToken { get; set; }
    public DateTimeOffset RefreshTokenExpiryTime { get; set; }
}
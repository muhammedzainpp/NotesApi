using Microsoft.AspNetCore.Identity;

namespace Notes.Infra.Models;

public class AppUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
}

using Microsoft.AspNetCore.Identity;

namespace Notes.Application;

public class AppUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
}

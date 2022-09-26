namespace Notes.Infra.Models;

public class JwtSettings
{
    public string SecurityKey { get; set; } = default!;
    public string ValidIssuer { get; set; } = default!;
    public string ValidAudience { get; set; } = default!;
    public string ExpiryInMinutes { get; set; } = default!;
}

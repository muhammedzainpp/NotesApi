using System.Security.Claims;

namespace Notes.Application.Common.Dtos.IdentityDtos;

public class AuthResponseDto
{
    public int UserId { get; set; }
    public string? AppUserId { get; set; }
    public string? FirstName { get; set; }
    public bool IsSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
    public List<Claim>? Claims { get; set; }
}

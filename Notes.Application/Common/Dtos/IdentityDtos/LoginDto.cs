namespace Notes.Application.Common.Dtos.IdentityDtos;

public class LoginDto
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

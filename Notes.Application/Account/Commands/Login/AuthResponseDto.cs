namespace Notes.Application.Account.Commands.Login;

public class AuthResponseDto
{
    public bool IsAuthSuccessful { get; set; }
    public string ErrorMessage { get; set; } = default!;
    public string Token { get; set; } = default!;
}

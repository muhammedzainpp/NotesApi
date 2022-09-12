using MediatR;
using Microsoft.AspNetCore.Identity;
using Notes.Application.Account.Commands.Login;

namespace Notes.Application.Account.Commands.Register;

public class RegisterCommand : IRequest<AuthResponseDto>
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMediator _mediator;

    public RegisterCommandHandler(UserManager<AppUser> userManager, IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new AppUser { UserName = request.Email, Email = request.Email };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded) return new AuthResponseDto { IsSuccessful =  result.Succeeded };

        var loginCommand = new LoginCommand
        {
            Email = request.Email,
            Password = request.Password,
            RememberMe  = false
        };

        return await _mediator.Send(loginCommand);
    }
}

using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Notes.Application.Account.Commands.Logout;

public class LogoutCommand : IRequest<Unit>
{

}
public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly SignInManager<AppUser> _signInManager;

    public LogoutCommandHandler(SignInManager<AppUser> signInManager) => 
        _signInManager = signInManager;

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();

        return Unit.Value;
    }
}



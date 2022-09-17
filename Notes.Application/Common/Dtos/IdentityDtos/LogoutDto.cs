using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Notes.Application.Common.Dtos.IdentityDtos;

public class LogoutDto : IRequest<Unit>
{

}
public class LogoutCommandHandler : IRequestHandler<LogoutDto, Unit>
{
    private readonly SignInManager<AppUser> _signInManager;

    public LogoutCommandHandler(SignInManager<AppUser> signInManager) =>
        _signInManager = signInManager;

    public async Task<Unit> Handle(LogoutDto request, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();

        return Unit.Value;
    }
}



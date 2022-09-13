using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Notes.Application.Account.Commands.Login;
using Notes.Application.Interfaces;

namespace Notes.Application.Account.Commands.Register;

public class RegisterCommand : IRequest<AuthResponseDto>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMediator _mediator;
    private readonly IAppDbContext _context;

    public RegisterCommandHandler(UserManager<AppUser> userManager, IMediator mediator,IAppDbContext context)
    {
        _userManager = userManager;
        _mediator = mediator;
        _context = context;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var appUser = new AppUser { UserName = request.Email, Email = request.Email };

        var result = await _userManager.CreateAsync(appUser, request.Password);

        if (!result.Succeeded) return new AuthResponseDto { IsSuccessful = result.Succeeded };

        appUser = await _userManager.FindByEmailAsync(request.Email);

        var user = MapToUser(appUser);

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        var loginCommand = MapToLoginCommand(request);

        return await _mediator.Send(loginCommand);
    }

    private static User MapToUser(AppUser appUser) => new()
    {
        AppUserId = appUser.Id,
        FirstName = appUser.UserName,
        LastName = appUser.UserName
    };

    private static LoginCommand MapToLoginCommand(RegisterCommand request) => new()
    {
        Email = request.Email,
        Password = request.Password,
        RememberMe = false
    };
}

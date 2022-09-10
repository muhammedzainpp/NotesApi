using MediatR;
using Microsoft.AspNetCore.Identity;
using Notes.Application.Notes.Commands;
using System.ComponentModel.DataAnnotations;

namespace Notes.Application.Account.Commands.Register;

public class RegisterCommand : IRequest<RegistrationResponseDto>
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegistrationResponseDto>
{
    private readonly UserManager<IdentityUser> _userManager;

    public RegisterCommandHandler(UserManager<IdentityUser> userManager) => 
        _userManager = userManager;

    public async Task<RegistrationResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new IdentityUser { UserName = request.Email, Email = request.Email };

        var result = await _userManager.CreateAsync(user, request.Password);


        var response = new RegistrationResponseDto
        {
            Errors = result.Errors.Select(e => e.Description), 
            IsSuccessfulRegistration = result.Succeeded
        };

        return response;
    }
}

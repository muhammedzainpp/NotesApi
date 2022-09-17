using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Api.Controllers.Base;
using Notes.Application.Common.Dtos.IdentityDtos;
using Notes.Application.Interfaces;

namespace Notes.Api.Controllers;

public class AccountController : ApiControllerBase
{
    private readonly IIdentityService _identityService;

    public AccountController(IMediator mediator, IIdentityService identityService) : base(mediator) => 
        _identityService = identityService;

    [HttpPost("Registration")]
    public async Task<IActionResult> RegisterUser(RegisterDto request)
    {
        var response = await _identityService.RegisterAsync(request);
        if (response.IsSuccessful)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync(LoginDto request)
    {
        var response = await _identityService.LoginAsync(request);
        if (response.IsSuccessful)
            return Ok(response);

        return BadRequest(response);
    }

    [Authorize]
    [HttpPost("Logout")]
    public IActionResult LogoutAsync(LogoutDto command) =>
        Ok(_mediator.Send(command));

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenDto request)
    {
        var response = await _identityService.RefreshTokenAsync(request);
        if (response.IsSuccessful)
            return Ok(response);

        return BadRequest(response);
    }
}

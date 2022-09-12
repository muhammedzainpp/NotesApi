using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.Api.Controllers.Base;
using Notes.Application;
using Notes.Application.Account.Commands.Login;
using Notes.Application.Account.Commands.Logout;
using Notes.Application.Account.Commands.Register;


namespace Notes.Api.Controllers;

public class AccountController : ApiControllerBase
{
    public AccountController(IMediator mediator) : base(mediator)
    { }

    [HttpPost("Registration")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterCommand command)
    {
        var response = await _mediator.Send(command);
        if (response.IsSuccessful)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var response = await _mediator.Send(command);
        if (response.IsSuccessful)
            return Ok(response);

        return BadRequest(response);
    }

    [Authorize]
    [HttpPost("Logout")]
    public IActionResult LogoutAsync(LogoutCommand command) =>
        Ok(_mediator.Send(command));

    [HttpGet("currentuserinfo")]
    public CurrentUserDto CurrentUserInfo()
    {
        return new CurrentUserDto
        {
            IsAuthenticated = User?.Identity?.IsAuthenticated ?? false,
            UserName = User?.Identity?.Name,
            Claims = User?.Claims
            .ToDictionary(c => c.Type, c => c.Value)
        };
    }
}
public class CurrentUserDto
{
    public bool IsAuthenticated { get; set; }
    public string? UserName { get; set; }
    public Dictionary<string, string>? Claims { get; set; }
}

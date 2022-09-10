using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notes.Api.Controllers.Base;
using Notes.Application.Account.Commands.Login;
using Notes.Application.Account.Commands.Register;


namespace Notes.Api.Controllers
{
    public class AccountController : ApiControllerBase
    {
        public AccountController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccessfulRegistration)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsAuthSuccessful)
                return Ok(response);

            return BadRequest(response);
        }
    }
}

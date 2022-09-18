using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notes.Api.Controllers.Base;
using Notes.Application.UserProfiles.Commands;
using Notes.Application.UserProfiles.Queries.GetUserProfileQuery;

namespace Notes.Api.Controllers;

public class UserProfileController : ApiControllerBase
{
    public UserProfileController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> SaveUserAsync(SaveUserProfileCommand request) =>
        Ok(await _mediator.Send(request));


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserAsync(int id) => 
        Ok(await _mediator.Send(new GetUserProfileQuery { Id = id }));
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notes.Api.Controllers.Base;

namespace Notes.Api.Controllers;

public class HealthController : ApiControllerBase
{
    public HealthController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet(Name = "HealthCheck")]
    public IActionResult Get() => Ok("Succeesfull");
}

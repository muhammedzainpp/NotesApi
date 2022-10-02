using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Api.Controllers.Base;
using Notes.Application.Labels.Commands;
using Notes.Application.Labels.Queries;

namespace Notes.Api.Controllers;

[Authorize]
public class LabelController : ApiControllerBase
{
    public LabelController(IMediator mediator) : base(mediator)
    { }
    [HttpGet]
    public async Task<IActionResult> GetLabels()
    {
        return Ok(await _mediator.Send(new GetLabelsQuery()));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetLabel(int id)
    {
        return Ok(await _mediator.Send(new GetLabelQuery() { Id = id }));
    }

    [HttpPost]
    public async Task<IActionResult> CreateNote(SaveLabelCommand request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteNote(int id)
    {
        return Ok(await _mediator.Send(new DeleteLabelCommand() { Id = id }));
    }
}

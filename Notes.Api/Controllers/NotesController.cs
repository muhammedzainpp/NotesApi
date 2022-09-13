using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Api.Controllers.Base;
using Notes.Application.Notes.Commands;
using Notes.Application.Notes.Queries;

namespace Notes.Api.Controllers;

[Authorize]
public class NotesController : ApiControllerBase
{
    public NotesController(IMediator mediator) : base(mediator)
    {}

    [HttpGet]
    public async Task<IActionResult> GetNotes() =>
        Ok(await _mediator.Send(new GetNotesQuery()));

    [HttpPost]
    public async Task<IActionResult> CreateNote(SaveNoteCommand request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteNote(int id)
    {
        return Ok(await _mediator.Send(new DeleteNoteCommand() { Id = id }));
    }
}

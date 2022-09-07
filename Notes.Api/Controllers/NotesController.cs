using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes.Commands;
using Notes.Application.Notes.Queries;

namespace Notes.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public NotesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetNotes()
        {
            return Ok(await _mediator.Send(new GetNotesQuery()));
        }

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
}

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.Api.Controllers.Base;
using Notes.Application.Labels.Commands;
using Notes.Application.Labels.Queries;
using Notes.Application.Notes.Commands;
using Notes.Application.Notes.Queries;

namespace Notes.Api.Controllers
{
    public class LabelController : ApiControllerBase
    {
        public LabelController(IMediator mediator) : base(mediator)
        { }
        [HttpGet]
        public async Task<IActionResult> GetLabels()
        {
            return Ok(await _mediator.Send(new GetLabelQuery()));
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
}

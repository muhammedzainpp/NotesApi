using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Interfaces;
using Notes.Shared.Dtos.Notes;

namespace Notes.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesRepository _notesRepository;
        public NotesController(INotesRepository notesrepository)
        {
            _notesRepository= notesrepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetNoteDto>>> GetNotes()
        {
            try
            {
                var notes = await this._notesRepository.GetNotes();
                if(notes == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(notes);
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        [HttpPost]
        public async Task<int>CreateNote(SaveNoteCommand note)
        {
            var id= await this._notesRepository.CreateNote(note);
            return id;

        }




    }
}

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;
using Notes.Shared.Dtos.Notes;

namespace Notes.Application
{
    public class NotesRepository : INotesRepository
    {
        private readonly IAppDbContext _context;
        public NotesRepository(IAppDbContext context) => _context = context;

        public async Task<int> CreateNote(SaveNoteCommand note)
        {
            var entity = new Note()
            {
                Id= note.Id,
                Description= note.Description,
                Title= note.Title,
            };

            _context.Notes.Add(entity);
           
            await _context.SaveChangesAsync();

            return entity.Id;
           



        }

        public async Task<IEnumerable<GetNoteDto>> GetNotes()
        {
            var notes = await _context.Notes.ToListAsync();

            var noteDto=notes.Select(n => new GetNoteDto
            {
                Id = n.Id,
                Title = n.Title,
                Description = n.Description
            });

            return noteDto;
        }     
    }
}

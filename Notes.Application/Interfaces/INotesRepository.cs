using Notes.Shared.Dtos.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Interfaces
{
    public interface INotesRepository

    {
        Task<IEnumerable<GetNoteDto>>GetNotes();
        Task<int> CreateNote(SaveNoteCommand note);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Shared.Dtos.Notes
{
    public class SaveNoteCommand
    {
        public int Id { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
    }
}

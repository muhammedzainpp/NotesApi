namespace Domain.Exceptions;

public class NoteNotFoundException : NotFoundException
{
    public NoteNotFoundException(int noteId)
        : base($"The note with the identifier {noteId} was not found.")
    {
    }

}

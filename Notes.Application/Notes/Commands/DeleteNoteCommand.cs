using Domain.Exceptions;
using Notes.Application.Common.Abstractions;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;

namespace Notes.Application.Notes.Commands;

public class DeleteNoteCommand : ICommand<int>
{
    public int Id { get; set; }
}
public class DeleteCommandHandler : ICommandHandler<DeleteNoteCommand, int>
{
    private readonly IAppDbContext _context;
    public DeleteCommandHandler(IAppDbContext context) => _context = context;
    public async Task<int> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {

        var result = await _context.Notes.FindAsync(request.Id);


        if (result == null)
            throw new NoteNotFoundException(request.Id);

        _context.Notes.Remove(result);
        await _context.SaveChangesAsync();
        return result.Id;
    }
}


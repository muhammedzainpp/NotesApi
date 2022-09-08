using MediatR;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;

namespace Notes.Application.Labels.Commands;

public class DeleteLabelCommand : IRequest
{
    public int Id { get; set; }
}
public class DeleteLabelHandler : IRequestHandler<DeleteLabelCommand, Unit>
{
    private readonly IAppDbContext _context;
    public DeleteLabelHandler(IAppDbContext context) => _context = context;
    public async Task<Unit> Handle(DeleteLabelCommand request, CancellationToken cancellationToken)
    {

        var result = await _context.Labels.FindAsync(request.Id);


        if (result == null)
            throw new NotFoundException($"Note not found with id {request.Id}");

        _context.Labels.Remove(result);
        await _context.SaveChangesAsync();
        return Unit.Value;
    }
}

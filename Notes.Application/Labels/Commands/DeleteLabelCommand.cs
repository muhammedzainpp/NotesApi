using Domain.Exceptions;
using Notes.Application.Common.Abstractions;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;

namespace Notes.Application.Labels.Commands;

public class DeleteLabelCommand : ICommand<int>
{
    public int Id { get; set; }
}
public class DeleteLabelHandler : ICommandHandler<DeleteLabelCommand, int>
{
    private readonly IAppDbContext _context;
    public DeleteLabelHandler(IAppDbContext context) => _context = context;
    public async Task<int> Handle(DeleteLabelCommand request, CancellationToken cancellationToken)
    {

        var result = await _context.Labels.FindAsync(request.Id);


        if (result == null)
            throw new LabelNotFoundException(request.Id);

        _context.Labels.Remove(result);
        await _context.SaveChangesAsync();
        return request.Id;
    }
}

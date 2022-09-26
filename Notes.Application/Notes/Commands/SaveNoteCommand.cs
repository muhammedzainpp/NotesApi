using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Notes.Application.Common.Abstractions;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;

namespace Notes.Application.Notes.Commands;

public class SaveNoteCommand : ICommand<int>, IMapTo<Note>
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public int UserId { get; set; }
}

public class SaveNoteCommandHandler : ICommandHandler<SaveNoteCommand, int>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    public SaveNoteCommandHandler(IAppDbContext context, IMapper mapper) =>
        (_context, _mapper) = (context, mapper);

    public async Task<int> Handle(SaveNoteCommand request, CancellationToken cancellationToken)
    {
        Note? entity;
        if (request.Id == 0)
        {
            entity = _mapper.Map<Note>(request);
            _context.Notes.Add(entity);
        }
        else
        {
            entity = await _context.Notes.FindAsync(request.Id);

            if (entity == null) throw new NoteNotFoundException(request.Id);

            _mapper.Map(request, entity);
        }
        //var entity = new Note()
        //{
        //    Id = request.Id,
        //    Description = request.Description,
        //    Title = request.Title,
        //};

        try
        {
            await _context.SaveChangesAsync(cancellationToken);

        }
        catch (Exception ex)
        {

            throw ex;
        }


        return entity.Id;

    }
}

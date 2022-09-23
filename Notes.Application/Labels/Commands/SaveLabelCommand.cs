using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Notes.Application.Common.Abstractions;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;

namespace Notes.Application.Labels.Commands;

public class SaveLabelCommand : ICommand<int>, IMapTo<Label>
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
}

public class SaveLabelCommandHandler : ICommandHandler<SaveLabelCommand, int>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    public SaveLabelCommandHandler(IAppDbContext context, IMapper mapper) =>
        (_context, _mapper) = (context, mapper);

    public async Task<int> Handle(SaveLabelCommand request, CancellationToken cancellationToken)
    {
        Label? entity;
        if (request.Id == 0)
        {
            entity = _mapper.Map<Label>(request);
            _context.Labels.Add(entity);
        }
        else
        {
            entity = await _context.Labels.FindAsync(request.Id);

            if (entity == null) throw new LabelNotFoundException(request.Id);

            _mapper.Map(request, entity);
        }
        //var entity = new Note()
        //{
        //    Id = request.Id,
        //    Description = request.Description,
        //    Title = request.Title,
        //};


        await _context.SaveChangesAsync();

        return entity.Id;

    }
}

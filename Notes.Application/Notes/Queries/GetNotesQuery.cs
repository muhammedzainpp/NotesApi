using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Abstractions;
using Notes.Application.Interfaces;
using Notes.Application.Notes.Queries.Dtos;

namespace Notes.Application.Notes.Queries;

public class GetNotesQuery : IQuery<IEnumerable<NotesDto>>
{
}

public class GetNotesQueryHandler : IQueryHandler<GetNotesQuery, IEnumerable<NotesDto>>
{

    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    public GetNotesQueryHandler(IAppDbContext context, IMapper mapper) =>
        (_context, _mapper) = (context, mapper);

    public async Task<IEnumerable<NotesDto>> Handle(GetNotesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Notes
             .ProjectTo<NotesDto>(_mapper.ConfigurationProvider).ToListAsync();


        //var noteDto = notes.Select(n => new NotesDto
        //{
        //    Id = n.Id,
        //    Title = n.Title,
        //    Description = n.Description
        //});

        //return noteDto;
    }
}

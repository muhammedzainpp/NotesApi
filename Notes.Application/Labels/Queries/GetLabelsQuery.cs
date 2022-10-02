using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Abstractions;
using Notes.Application.Interfaces;
using Notes.Application.Labels.Queries.Dtos;

namespace Notes.Application.Labels.Queries;

public class GetLabelsQuery : IQuery<IEnumerable<GetLabelsQueryDto>>
{
    int Id { get; set; } 
}

public class GetLabelsQueryHandler : IQueryHandler<GetLabelsQuery, IEnumerable<GetLabelsQueryDto>>
{

    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    public GetLabelsQueryHandler(IAppDbContext context, IMapper mapper) =>
        (_context, _mapper) = (context, mapper);

    public async Task<IEnumerable<GetLabelsQueryDto>> Handle(GetLabelsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Labels
        .ProjectTo<GetLabelsQueryDto>(_mapper.ConfigurationProvider).ToListAsync();

    }
}

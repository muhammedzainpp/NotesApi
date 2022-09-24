using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Abstractions;
using Notes.Application.Interfaces;
using Notes.Application.Labels.Queries.Dtos;

namespace Notes.Application.Labels.Queries;

public class GetLabelQuery : IQuery<IEnumerable<LabelDto>>
{
}

public class GetLabelQueryHandler : IQueryHandler<GetLabelQuery, IEnumerable<LabelDto>>
{

    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    public GetLabelQueryHandler(IAppDbContext context, IMapper mapper) =>
        (_context, _mapper) = (context, mapper);

    public async Task<IEnumerable<LabelDto>> Handle(GetLabelQuery request, CancellationToken cancellationToken)
    {
        return await _context.Labels
        .ProjectTo<LabelDto>(_mapper.ConfigurationProvider).ToListAsync();

    }
}

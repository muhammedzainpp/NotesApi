using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;
using Notes.Application.Labels.Queries.Dtos;

namespace Notes.Application.Labels.Queries;

public class GetLabelQuery : IRequest<IEnumerable<LabelDto>>
{
}

public class GetLabelQueryHandler : IRequestHandler<GetLabelQuery, IEnumerable<LabelDto>>
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

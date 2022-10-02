using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Abstractions;
using Notes.Application.Interfaces;
using Notes.Application.Labels.Queries.Dtos;
using Notes.Application.UserProfiles.Queries.GetUserProfileQuery;

namespace Notes.Application.Labels.Queries;
public class GetLabelQuery : IQuery<GetLabelQueryDto>
{
    public int Id { get; set; }
}

public class GetLabelQueryHandler : IQueryHandler<GetLabelQuery, GetLabelQueryDto>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetLabelQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetLabelQueryDto> Handle(GetLabelQuery request, CancellationToken cancellationToken)
    {
        var label = await _context.Labels
             .ProjectTo<GetLabelQueryDto>(_mapper.ConfigurationProvider)
             .SingleAsync(n => n.Id == request.Id);

        return label;

    }
}

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Abstractions;
using Notes.Application.Interfaces;

namespace Notes.Application.UserProfiles.Queries.GetUserProfileQuery;

public class GetUserProfileQuery : IQuery<GetUserProfileDto>
{
    public int Id { get; set; }
}

public class GetUserProfileQueryHandler : IQueryHandler<GetUserProfileQuery, GetUserProfileDto>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetUserProfileQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetUserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .ProjectTo<GetUserProfileDto>(_mapper.ConfigurationProvider)
            .SingleAsync(n => n.Id == request.Id);

        return user;
    }
}

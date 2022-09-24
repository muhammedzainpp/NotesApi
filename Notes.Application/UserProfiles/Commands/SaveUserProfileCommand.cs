using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Notes.Application.Common.Abstractions;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;

namespace Notes.Application.UserProfiles.Commands;

public class SaveUserProfileCommand : ICommand<int>, IMapTo<User>
{
    public int Id { get; set; }
    public string AppUserId { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; }
    public string? About { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? TwitterUrl { get; set; }
    public string? FacebookUrl { get; set; }
    public string? InstagamUrl { get; set; }

}
public class SaveUserProfileCommandHandler : ICommandHandler<SaveUserProfileCommand, int>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    public SaveUserProfileCommandHandler(IAppDbContext context, IMapper mapper) =>
        (_context, _mapper) = (context, mapper);

    public async Task<int> Handle(SaveUserProfileCommand request, CancellationToken cancellationToken)
    {
        User? entity;
        if (request.Id == 0)
        {
            entity = _mapper.Map<User>(request);
            _context.Users.Add(entity);
        }
        else
        {
            entity = await _context.Users.FindAsync(request.Id);

            if (entity == null) throw new UserNotFoundException(request.Id);

            _mapper.Map(request, entity);
        }


        _ = await _context.SaveChangesAsync();

        return entity.Id;

    }
}

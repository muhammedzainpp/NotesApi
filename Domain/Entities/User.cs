using Domain.Entities.Base;

namespace Domain.Entities;

public class User : EntityBase
{
    public string AppUserId { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; }
}

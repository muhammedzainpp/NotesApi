using Domain.Entities.Base;

namespace Domain.Entities;

public class Note : EntityBase
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = default!;
}

using Domain.Entities.Base;

namespace Domain.Entities;

public class City : EntityBase
{
    public string Name { get; set; } = default!;
    public State State { get; set; } = default!;
    public int StateId { get; set; }
}

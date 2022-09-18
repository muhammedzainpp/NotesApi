using Domain.Entities.Base;

namespace Domain.Entities;

public class State : EntityBase
{
    public string Name { get; set; } = default!;
    public int CountryId { get; set; }
    public Country Country { get; set; }=default!;
}

using Domain.Entities.Base;

namespace Domain.Entities;

public class Country : EntityBase
{
    public string Name { get; set; } = default!;
}

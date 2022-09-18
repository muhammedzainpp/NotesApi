using Domain.Entities.Base;

namespace Domain.Entities;

public class Address : EntityBase
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    public int CityId { get; set; }
    public City City { get; set; } = default!;
    public string? LandMark { get; set; }
}

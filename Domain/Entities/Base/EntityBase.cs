namespace Domain.Entities.Base;

public class EntityBase
{
    public int Id { get; set; }
    public int CreatedBy { get; set; }
    public int ModifiedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public bool IsDeleted { get; set; }
}

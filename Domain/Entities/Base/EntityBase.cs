namespace Domain.Entities.Base;

public class EntityBase
{
    public int Id { get; set; }
    public string CreatedBy { get; set; } = default!;
    public string? ModifiedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedAt { get; set; }
    public bool IsDeleted { get; set; }
}

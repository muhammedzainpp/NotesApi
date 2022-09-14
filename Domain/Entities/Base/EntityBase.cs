﻿namespace Domain.Entities.Base;

public class EntityBase
{
    public int Id { get; set; }
    public string CreatedBy { get; set; } = default!;
    public string? ModifiedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public bool IsDeleted { get; set; }
}

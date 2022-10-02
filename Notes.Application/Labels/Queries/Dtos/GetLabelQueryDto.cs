using Domain.Entities;
using Notes.Application.Interfaces;

namespace Notes.Application.Labels.Queries.Dtos;

public class GetLabelQueryDto : IMapFrom<Label>
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
}

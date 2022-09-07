using Domain.Entities;
using Notes.Application.Interfaces;

namespace Notes.Application.Notes.Queries.Dtos;

public class NotesDto : IMapFrom<Note>
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
}

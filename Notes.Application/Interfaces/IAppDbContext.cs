using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Notes.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Note> Notes { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
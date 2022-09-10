using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Notes.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Note> Notes { get; set; }
        DbSet<Label> Labels { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
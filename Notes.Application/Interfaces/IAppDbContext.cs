using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Notes.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Note> Notes { get; set; }
        DbSet<Label> Labels { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<Country> Countries { get; set; }
        DbSet<State> States { get; set; }
        DbSet<City> Cities { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
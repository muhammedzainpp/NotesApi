using Domain.Entities;
using Domain.Entities.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notes.Application;
using Notes.Application.Interfaces;


namespace Notes.Infra.Data;

public class AppDbContext : IdentityDbContext<AppUser>, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Note> Notes { get; set; } = default!;
    public DbSet<Label> Labels { get ; set ; } = default!;
    public DbSet<User> Users { get; set; } = default!;

    public override int SaveChanges()
    {
        OnSave();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnSave();

        return base.SaveChangesAsync(cancellationToken);
    }

    private void OnSave()
    {
        var now = DateTime.UtcNow;

        foreach (var changedEntity in ChangeTracker.Entries())
        {
            if (changedEntity.Entity is EntityBase entity)
            {
                switch (changedEntity.State)
                {
                    case EntityState.Added:
                        entity.CreatedDate = now;
                        entity.ModifiedAt = now;
                        entity.CreatedBy = "zain";
                        entity.ModifiedBy = "zain";
                        break;
                    case EntityState.Modified:
                        Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                        entity.ModifiedAt = now;
                        entity.ModifiedBy = "shaheem";
                        break;
                    case EntityState.Deleted:
                        changedEntity.State = EntityState.Modified;
                        changedEntity.CurrentValues["IsDeleted"] = true;
                        break;

                }
            }
        }
    }
}

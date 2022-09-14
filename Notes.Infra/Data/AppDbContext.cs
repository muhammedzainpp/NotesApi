using Domain.Entities;
using Domain.Entities.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notes.Application;
using Notes.Application.Interfaces;


namespace Notes.Infra.Data;

public class AppDbContext : IdentityDbContext<AppUser>, IAppDbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public DbSet<Note> Notes { get; set; } = default!;
    public DbSet<Label> Labels { get ; set ; } = default!;
    public new DbSet<User> Users { get; set; } = default!;

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
        var currentUser = GetCurrentUser();

        foreach (var changedEntity in ChangeTracker.Entries())
        {
            if (changedEntity.Entity is EntityBase entity)
            {
                switch (changedEntity.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = now;
                        entity.ModifiedAt = now;
                        entity.CreatedBy = currentUser;
                        entity.ModifiedBy = currentUser;
                        break;
                    case EntityState.Modified:
                        Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        Entry(entity).Property(x => x.CreatedAt).IsModified = false;
                        entity.ModifiedAt = now;
                        entity.ModifiedBy = currentUser;
                        break;
                    case EntityState.Deleted:
                        changedEntity.State = EntityState.Modified;
                        changedEntity.CurrentValues["IsDeleted"] = true;
                        break;

                }
            }
        }
    }

    private string GetCurrentUser()
    {
        var currentSessionUserEmail = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        var user = Users.SingleOrDefault(u => u.Email.Equals(currentSessionUserEmail));

        return user is not null ? user.FirstName : "Anonymous";
    }
}

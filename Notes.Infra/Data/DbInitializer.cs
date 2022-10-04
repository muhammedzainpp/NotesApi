using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notes.Application.Interfaces;
using Notes.Infra.Seedings;

namespace Notes.Infra.Data;

public class DbInitializer
{
    public static void MigrateDb(IHost? app)
    {
        var context = GetContext(app);
        context?.Database.Migrate();
    }


    public static async Task SeedAsync(IHost? app)
    {
        var context = GetContext(app);

        if (context is null) return;

        await SeedUserAsync(context);

        await SeedNotesAsync(context);
    }

    private static async Task SeedUserAsync(AppDbContext context)
    {
        if (context.Users.Any()) return;

        await context.Users.AddAsync(new User
        {
            AppUserId = "TestAppUserId",
            Email = "test@test.com",
            FirstName = "Tester",
            LastName = "Tester"
        });

        await context.SaveChangesAsync();
    }

    private static async Task SeedNotesAsync(AppDbContext context)
    {
        if (context.Notes.Any()) return;

        var notes = SeedHelper.SeedData<Note>("Notes.json");

        if (notes == null) return;

        await context.Notes.AddRangeAsync(notes);

        await context.SaveChangesAsync();
    }
    private static AppDbContext? GetContext(IHost? app)
    {
        var scopeFactory = app?.Services.GetService<IServiceScopeFactory>();
        var scope = scopeFactory?.CreateScope();
        var context = scope?.ServiceProvider.GetService<IAppDbContext>() as AppDbContext;

        return context;
    }
}

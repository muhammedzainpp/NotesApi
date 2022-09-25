using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notes.Application.Interfaces;

namespace Notes.Infra.Data;

public class DbInitializer
{
    public static void MigrateDb(IHost? app)
    {
        var scopeFactory = app?.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory?.CreateScope();
        var context = scope?.ServiceProvider.GetService<IAppDbContext>() as AppDbContext;
        context?.Database.Migrate();
    }
   
}

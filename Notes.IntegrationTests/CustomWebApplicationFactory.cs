using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using Notes.Application.Interfaces;
using Notes.Infra.Data;
using Notes.Infra.Seedings;
using Notes.IntegrationTests.Mocks;

namespace Notes.IntegrationTests;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("test");

        builder.ConfigureServices(services =>
        {
            var context = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<AppDbContext>));

            if (context is not null)
                services.Remove(context);

            services.AddScoped<ISeeder, MockSeeder>();

            services.AddScoped<ILoggedInUserInfo, MockLoggedInUser>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();

                db.Database.EnsureCreated();
                //db.Database.Migrate();
            }
        });
    }
}

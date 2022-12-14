using Notes.Application.DependencyInjection;
using Notes.Infra.Data;
using Notes.Infra.DependencyInjections;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var services = builder.Services;
{

    services.AddControllers();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddInfra(builder.Configuration);
    services.AddApplication();

    services.AddCors(options =>
    {
        options.AddPolicy(name: "AllowOrigin",
            builder =>
            {
                builder.WithOrigins("https://localhost:44373", "https://localhost:7282")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
            });
    });
}

var app = builder.Build();
{
    app.UseAuthentication();
    app.UseAuthorization();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        DbInitializer.MigrateDb(app);
        await DbInitializer.SeedAsync(app);
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("AllowOrigin");

    app.UseHttpsRedirection();


    app.MapControllers();

    app.Run();
}

public partial class Program { }
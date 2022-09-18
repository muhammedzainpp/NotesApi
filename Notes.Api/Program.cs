using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Notes.Application;
using Notes.Application.Interfaces;
using Notes.Infra.Data;
using Notes.Infra.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContext<IAppDbContext, AppDbContext>(
        options => options.UseSqlServer("name=ConnectionStrings:NotesDB"));

services.AddIdentity<AppUser,IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<DbInitializer>();

var assembly = AppDomain.CurrentDomain.GetAssemblies().
    Where(a => a.GetName()?.Name?.Equals("Notes.Application") ?? false).First();
builder.Services.AddMediatR(assembly);

var jwtSettings = builder.Configuration.GetSection("JWTSettings");
services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["securityKey"]))
    };
});

services.AddScoped<IIdentityService, IdentityService>();

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

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

static void InitializeDb(IHost? app)
{
    var scopeFactory = app?.Services.GetService<IServiceScopeFactory>();
    using (var scope = scopeFactory?.CreateScope())
    {
        var dbInitializer = scope?.ServiceProvider.GetService<DbInitializer>();

        dbInitializer?.Seed();

        var context = scope?.ServiceProvider.GetService<IAppDbContext>() as AppDbContext;

        context?.Database.Migrate();
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    InitializeDb(app);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowOrigin");

app.UseHttpsRedirection();


app.MapControllers();

app.Run();

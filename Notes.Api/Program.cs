using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Notes.Application.Interfaces;
using Notes.Infra.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<IAppDbContext, AppDbContext>(
        options => options.UseSqlServer("name=ConnectionStrings:NotesDB"));
builder.Services.AddIdentity<IdentityUser,IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<DbInitializer>();

var assembly = AppDomain.CurrentDomain.GetAssemblies().
    Where(a => a.GetName()?.Name?.Equals("Notes.Application") ?? false).First();
builder.Services.AddMediatR(assembly);


var jwtSettings = builder.Configuration.GetSection("JWTSettings");
builder.Services.AddAuthentication(opt =>
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


builder.Services.AddCors(options =>
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




void SeedData(IHost? app)
{
    var scopeFactory = app?.Services.GetService<IServiceScopeFactory>();
    using (var scope = scopeFactory?.CreateScope())
    {
        var service = scope?.ServiceProvider.GetService<DbInitializer>();

        service?.Seed();
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    SeedData(app);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowOrigin");

app.UseHttpsRedirection();


app.MapControllers();

app.Run();

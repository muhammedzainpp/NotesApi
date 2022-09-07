using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application;
using Notes.Application.Interfaces;
using Notes.Infra.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<IAppDbContext, AppDbContext>(
        options => options.UseSqlServer("name=ConnectionStrings:NotesDB"));

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var assembly = AppDomain.CurrentDomain.GetAssemblies().
    Where(a => a.GetName()?.Name?.Equals("Notes.Application") ?? false).First();
builder.Services.AddMediatR(assembly);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

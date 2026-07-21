using System.Diagnostics;
using GameServer.Data;
using GameServer.Repositories;
using GameServer.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();

#region db
var connectionString = 
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No ConnectionString:DefaultConnection Setting");

builder.Services.AddDbContext<GameDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});
#endregion

builder.Services.AddScoped<PlayerRepository>();
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
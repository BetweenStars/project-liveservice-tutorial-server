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
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? throw new InvalidOperationException("No DATABASE_URL Variable");

var uri = new Uri(databaseUrl);
var userInfo = uri.UserInfo.Split(":", 2);

var connectionString = new NpgsqlConnectionStringBuilder
{
    Host = uri.Host,
    Port = uri.Port,
    Username = Uri.UnescapeDataString(userInfo[0]),
    Password = Uri.UnescapeDataString(userInfo[1]),
    Database = uri.AbsolutePath.TrimStart('/'),
    SslMode = SslMode.Require
}.ConnectionString;

builder.Services.AddDbContext<GameDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});
#endregion

builder.Services.AddScoped<PlayerRepository>();
builder.Services.AddScoped<PlayerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
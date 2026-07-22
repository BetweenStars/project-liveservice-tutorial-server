using System.Diagnostics;
using FirebaseAdmin;
using GameServer.Data;
using GameServer.Repositories;
using GameServer.Services;
using Google.Apis.Auth.OAuth2;
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

#region auth
builder.Services.AddScoped<AuthService>();

FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.GetApplicationDefault(),
    ProjectId = "multigame-60a72"
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
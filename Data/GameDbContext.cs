using Microsoft.EntityFrameworkCore;
using GameServer.Models;

namespace GameServer.Data;

public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options)
        : base(options)
    {
    }

    public DbSet<Player> Players => Set<Player>();
}
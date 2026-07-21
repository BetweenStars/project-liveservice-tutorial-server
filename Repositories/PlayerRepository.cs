using GameServer.Data;
using GameServer.Models;
using Microsoft.EntityFrameworkCore;

namespace GameServer.Repositories;

public class PlayerRepository
{
    private readonly GameDbContext _db;

    public PlayerRepository(GameDbContext db)
    {
        _db = db;
    }

    public async Task<Player?> GetByUidAsync(string uid)
    {
        return await _db.Players
            .FirstOrDefaultAsync(x => x.FirebaseUid == uid);
    }

    public async Task SaveAsync(Player player)
    {
        _db.Players.Update(player);
        await _db.SaveChangesAsync();
    }
}
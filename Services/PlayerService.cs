using GameServer.Models;
using GameServer.Repositories;

namespace GameServer.Services;

public class PlayerService
{
    private readonly PlayerRepository _repository;

    private readonly ILogger<PlayerService> _logger;

    public PlayerService(PlayerRepository repository, ILogger<PlayerService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Player?> GetPlayerAsync(string uid)
    {
        return await _repository.GetByUidAsync(uid);
    }

    public async Task SavePlayerAsync(Player player)
    {
        await _repository.SaveAsync(player);
    }

    public async Task UpdatePlayerPositionAsync(PlayerPositionRequest request)
    {
        _logger.LogInformation(
            "Position : {PlayerId} ({X}, {Y}, {Z})",
            request.PlayerId,
            request.X,
            request.Y,
            request.Z
            );
    }
}
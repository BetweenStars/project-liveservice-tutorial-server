using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using GameServer.Data;
using GameServer.Models;

public class AuthService
{
    private readonly GameDbContext _dbContext;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        GameDbContext dbContext,
        ILogger<AuthService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Player> AuthenticateAsync(
        string idToken)
    {
        // Unity에서 받은 Firebase ID 토큰을 검증합니다.
        FirebaseToken decodedToken =
            await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);

        // Firebase에서 발급한 고유 사용자 ID입니다.
        string firebaseUid = decodedToken.Uid;

        Player? player = await _dbContext.Players
            .FirstOrDefaultAsync(x => x.FirebaseUid == firebaseUid);

        // 처음 접속한 사용자라면 Player 데이터를 생성합니다.
        if (player == null)
        {
            player = new Player
            {
                FirebaseUid = firebaseUid,
                Nickname = "처음 온 사람"
            };

            _dbContext.Players.Add(player);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation(
                "새 플레이어 생성: PlayerUid={PlayerUid}, FirebaseUid={FirebaseUid}",
                player.Id,
                firebaseUid);
        }

        return player;
    }
}
using GameServer.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        AuthService authService,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
    {
        _logger.LogInformation(request.IdToken);

        if (string.IsNullOrWhiteSpace(request.IdToken))
        {
            return BadRequest("Firebase Token Needed");
        }

        try
        {
            Player player = await _authService.AuthenticateAsync(request.IdToken);

            return Ok(new AuthResponse
            {
                Id = player.Id,
                NickName = player.Nickname
            });
        }
        catch (FirebaseAdmin.Auth.FirebaseAuthException e)
        {
            _logger.LogWarning(
                e,
                "Firebase Token Authorize Failed"
            );

            return Unauthorized("유효하지 않은 Firebase 토큰입니다");
        }
    }
}
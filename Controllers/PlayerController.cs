using FirebaseAdmin.Auth;
using GameServer.Data;
using GameServer.Models;
using GameServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameServer.Controllers;

[ApiController]
[Route("api/player")]
public class PlayerController : ControllerBase
{
    private readonly PlayerService _service;

    public PlayerController(PlayerService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlayer()
    {
        string? authorization = Request.Headers.Authorization;

        if (string.IsNullOrWhiteSpace(authorization) ||
            !authorization.StartsWith("Bearer "))
        {
            return Unauthorized();
        }

        string token = authorization["Bearer ".Length..];

        try
        {
            FirebaseToken decodedToken =
                await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);

            string firebaseUid = decodedToken.Uid;

            return Ok(new
            {
                FirebaseUid = firebaseUid
            });
        }
        catch
        {
            return Unauthorized();
        }
    }

    [HttpPost("position")]
    public async Task<IActionResult> UpdatePlayerPosition([FromBody] PlayerPositionRequest request)
    {
        Console.WriteLine("post dochack");
        await _service.UpdatePlayerPositionAsync(request);

        return Ok();
    }
}
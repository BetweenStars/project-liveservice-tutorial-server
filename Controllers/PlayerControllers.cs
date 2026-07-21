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

    [HttpGet("{Uid}")]
    public async Task<IActionResult> GetPlayer(string uid)
    {
        var player = await _service.GetPlayerAsync(uid);

        if (player == null)

            return NotFound();

        return Ok(player);
    }

    [HttpPost("save")]
    public async Task<IActionResult> SavePlayer([FromBody] Player player)
    {
        await _service.SavePlayerAsync(player);

        return Ok();
    }

    [HttpPost("position")]
    public async Task<IActionResult> UpdatePlayerPosition([FromBody] PlayerPositionRequest request)
    {
        Console.WriteLine("post dochack");
        await _service.UpdatePlayerPositionAsync(request);

        return Ok();
    }
}
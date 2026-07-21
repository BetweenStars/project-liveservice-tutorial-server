namespace GameServer.Models;

public class Player
{
    public long Id { get; set; }

    public string FirebaseUid { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
}
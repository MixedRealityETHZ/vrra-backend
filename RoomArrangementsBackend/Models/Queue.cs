namespace RoomArrangementsBackend.Models;

public class QueueItem
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
    
    public int AssetId { get; set; }
    public Asset Asset { get; set; } = null!;
}

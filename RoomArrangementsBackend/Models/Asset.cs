using System.Text.Json.Serialization;

namespace RoomArrangementsBackend.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AssetStatus
{
    Uploading,
    Ready,
}

public class Asset
{
    public int Id { get; private set; }
    
    public AssetStatus Status { get; set; }
}

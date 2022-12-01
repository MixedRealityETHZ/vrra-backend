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
    
    public DateTime Created { get; set; } = DateTime.UtcNow;
    
    public DateTime Updated { get; set; } = DateTime.UtcNow;
}

public class AssetDto
{
    public int Id { get; set; }
    
    public AssetStatus Status { get; set; }
    
    public DateTime Created { get; set; }
    
    public DateTime Updated { get; set; }
    
    public string? Url { get; set; }
    
    public AssetDto(Asset asset, string? url)
    {
        Id = asset.Id;
        Status = asset.Status;
        Created = asset.Created;
        Updated = asset.Updated;
        Url = url;
    }
}
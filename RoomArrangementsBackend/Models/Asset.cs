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

    public string Name { get; set; } = "";

    public AssetStatus Status { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public DateTime Updated { get; set; } = DateTime.UtcNow;
}

public class AssetDto
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public AssetStatus Status { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public string? Url { get; set; }

    public AssetDto(Asset asset, string? url)
    {
        Id = asset.Id;
        Name = asset.Name;
        Status = asset.Status;
        Created = asset.Created;
        Updated = asset.Updated;
        Url = url;
    }
}

public class AddAssetBody
{
    public string Name { get; set; } = "";
}

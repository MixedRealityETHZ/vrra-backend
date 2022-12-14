using System.ComponentModel.DataAnnotations.Schema;

namespace RoomArrangementsBackend.Models;

public class Model
{
    public int Id { get; private set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public Bounds3 Bounds { get; set; } = new();

    public int AssetId { get; set; }
    public Asset Asset { get; set; }

    public int? ThumbnailAssetId { get; set; }
    public Asset? ThumbnailAsset { get; set; }
}

public class ModelDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public Bounds3 Bounds { get; set; }

    public int AssetId { get; set; }

    public int? ThumbnailAssetId { get; set; }

    public ModelDto(Model model)
    {
        Id = model.Id;
        Name = model.Name;
        AssetId = model.AssetId;
        Path = model.Path;
        Bounds = model.Bounds;
        ThumbnailAssetId = model.ThumbnailAssetId;
    }
}

public class AddModelBody
{
    public string Name { get; set; }

    public int AssetId { get; set; }

    public string Path { get; set; }

    public Bounds3? Bounds { get; set; }

    public int? ThumbnailAssetId { get; set; }
}
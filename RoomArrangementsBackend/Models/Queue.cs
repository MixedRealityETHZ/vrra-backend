using System.Text.Json.Serialization;

namespace RoomArrangementsBackend.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QueueItemStatus
{
    Pending,
    InProgress,
    Completed,
    Failed
}

public class QueueItemDto
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public QueueItemStatus Status { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public DateTime? Completed { get; set; }

    public DateTime? Started { get; set; }

    public string Message { get; set; }

    public int AssetId { get; set; }

    public int PointCloudAssetId { get; set; }

    public string Path { get; set; }

    public QueueItemDto(QueueItem item)
    {
        Id = item.Id;
        Name = item.Name;
        Status = item.Status;
        Created = item.Created;
        Started = item.Started;
        Completed = item.Completed;
        AssetId = item.AssetId;
        PointCloudAssetId = item.PointCloudAssetId;
        Message = item.Message;
        Path = item.Path;
    }
}

public class QueueItem
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public QueueItemStatus Status { get; set; } = QueueItemStatus.Pending;

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public DateTime? Started { get; set; }

    public DateTime? Completed { get; set; }

    public string Message { get; set; } = "";

    public int AssetId { get; set; }
    public Asset Asset { get; set; } = null!;

    public int PointCloudAssetId { get; set; }
    public Asset PointCloudAsset { get; set; } = null!;

    public string Path { get; set; } = "";
}

public class PushQueueBody
{
    public string Name { get; set; } = "";

    public int AssetId { get; set; }

    public int PointCloudAssetId { get; set; }

    public string Path { get; set; } = "";
}

public class CompleteQueueItemBody
{
    public string Message { get; set; } = "";
}
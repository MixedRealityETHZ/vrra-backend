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
    
    public QueueItemDto ToDto()
    {
        return new QueueItemDto
        {
            Id = Id,
            Name = Name,
            Status = Status,
            Created = Created,
            Started = Started,
            Completed = Completed,
            AssetId = AssetId,
            Message = Message
        };
    }
}

public class QueueItemPostBody
{
    public string Name { get; set; } = "";
}

public class QueueItemCompleteBody
{
    public string Message { get; set; } = "";
}

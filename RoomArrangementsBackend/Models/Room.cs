namespace RoomArrangementsBackend.Models;

public class Room
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public List<Obj> Objects { get; set; } = new();
}

public class RoomDto
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
    
    public RoomDto(Room room)
    {
        Id = room.Id;
        Name = room.Name;
    }
}

public class RoomPostBody
{
    public string Name { get; set; } = "";
}

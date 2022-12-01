using Microsoft.EntityFrameworkCore;

namespace RoomArrangementsBackend.Models;

[Owned]
public class Vector3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

[Owned]
public class Quaternion
{
    public float A { get; set; }
    public float B { get; set; }
    public float C { get; set; }
    public float D { get; set; }
}

public class Obj
{
    public int Id { get; private set; }

    public Vector3 Translation { get; set; } = new();

    public Quaternion Rotation { get; set; } = new();

    public Vector3 Scale { get; set; } = new();
        
    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;
        
    public int ModelId { get; set; }
    public Model Model { get; set; } = null!;
}

public class ObjDto{
    public int Id { get; set; }
    public Vector3 Translation { get; set; }
    public Quaternion Rotation { get; set; }
    public Vector3 Scale { get; set; }
    public int RoomId { get; set; }
    public int ModelId { get; set; }
    
    public ObjDto(Obj obj) 
    {
        Id = obj.Id;
        Translation = obj.Translation;
        Rotation = obj.Rotation;
        Scale = obj.Scale;
        RoomId = obj.RoomId;
        ModelId = obj.ModelId;
    }
}

public class AddRoomObjBody
{
    public Vector3 Translation { get; set; }
    public Quaternion Rotation { get; set; }
    public Vector3 Scale { get; set; }
    public int ModelId { get; set; }
}

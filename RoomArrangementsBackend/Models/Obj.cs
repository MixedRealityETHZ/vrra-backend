namespace RoomArrangementsBackend.Models;

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
    public ModelDto Model { get; set; }
    
    public ObjDto(Obj obj) 
    {
        Id = obj.Id;
        Translation = obj.Translation;
        Rotation = obj.Rotation;
        Scale = obj.Scale;
        RoomId = obj.RoomId;
        Model = new ModelDto(obj.Model);
    }
}

public class AddRoomObjBody
{
    public Vector3 Translation { get; set; }
    public Quaternion Rotation { get; set; }
    public Vector3 Scale { get; set; }
    public int ModelId { get; set; }
}

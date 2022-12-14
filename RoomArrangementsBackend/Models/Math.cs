namespace RoomArrangementsBackend.Models;

using Microsoft.EntityFrameworkCore;

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
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }
}

[Owned]
public class Bounds3
{
    public Vector3 PMin { get; set; } = new() { X = 0, Y = 0, Z = 0 };
    public Vector3 PMax { get; set; } = new() { X = 0, Y = 0, Z = 0 };
}
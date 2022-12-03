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
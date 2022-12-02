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
    public float A { get; set; }
    public float B { get; set; }
    public float C { get; set; }
    public float D { get; set; }
}
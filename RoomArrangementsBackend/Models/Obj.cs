using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomArrangementsBackend.Models
{
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

    [Table("Object")]
    public class Obj
    {
        public int Id { get; private set; }

        public Vector3 Translation { get; set; } = new();

        public Quaternion Rotation { get; set; } = new();

        public Model Model { get; set; } = null!;

    }
}

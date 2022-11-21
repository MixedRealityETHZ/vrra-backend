namespace RoomArrangementsBackend.Models
{
    public struct Vector3 { public float X, Y, Z; }

    public struct Quaternion { public float a, b, c, d; }

    public class Obj
    {
        public int Id { get; private set; }

        public Vector3 Translation { get; set; }

        public Quaternion Quaternion { get; set; }

        public Model Model { get; set; } = null!;

    }
}

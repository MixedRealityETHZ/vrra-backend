namespace RoomArrangementsBackend.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public List<Obj> Objects { get; set; } = new();

    }
}

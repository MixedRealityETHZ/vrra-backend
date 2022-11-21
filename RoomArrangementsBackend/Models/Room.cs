namespace RoomArrangementsBackend.Models
{
    public class Room
    {
        public string Name { get; set; } = "";
        public List<Obj> Objects { get; set; } = new();

    }
}

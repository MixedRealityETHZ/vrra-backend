using System.ComponentModel.DataAnnotations.Schema;

namespace RoomArrangementsBackend.Models;

[Table("Model")]
public class Model
{
    public int Id { get; private set; }
    
    public string Name { get; set; }
    
    public int AssetId { get; set; }
    public Asset Asset { get; set; }
}
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

public class ModelDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public Asset Asset { get; set; }
    
    public ModelDto(Model model)
    {
        Id = model.Id;
        Name = model.Name;
        Asset = model.Asset;
    }
}

public class AddModelBody
{
    public string Name { get; set; }
    
    public int AssetId { get; set; }
}

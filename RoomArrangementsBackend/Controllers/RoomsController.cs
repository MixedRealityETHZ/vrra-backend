using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomArrangementsBackend.Data;
using RoomArrangementsBackend.Models;

namespace RoomArrangementsBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomsController : ControllerBase
{
    private readonly ILogger<RoomsController> _logger;
    private readonly DataContext _context;

    public RoomsController(ILogger<RoomsController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetRooms()
    {
        var rooms = await _context.Rooms.Select(r => new RoomDto(r)).ToListAsync();
        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRoom(int id)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
        if (room == null)
        {
            return NotFound();
        }

        return Ok(new RoomDto(room));
    }

    [HttpPost]
    public async Task<IActionResult> AddRoom([FromBody] RoomPostBody body)
    {
        Asset? thumbnail = null;
        if (body.ThumbnailAssetId != null)
        {
            thumbnail = await _context.Assets.FirstOrDefaultAsync(x =>
                x.Id == body.ThumbnailAssetId && x.Status == AssetStatus.Ready);
            if (thumbnail == null)
            {
                return BadRequest("Thumbnail not found or not ready");
            }
        }

        var room = new Room
        {
            Name = body.Name,
            ThumbnailAsset = thumbnail
        };
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, new RoomDto(room));
    }

    [HttpPost("{id:int}/objects")]
    public async Task<IActionResult> AddRoomObject(int id, [FromBody] AddRoomObjBody body)
    {
        var room = await _context.Rooms.Include(x => x.Objects).FirstOrDefaultAsync(x => x.Id == id);
        if (room == null)
        {
            return NotFound();
        }

        var model = await _context.Models.FirstOrDefaultAsync(x => x.Id == body.ModelId);
        if (model == null)
        {
            return BadRequest();
        }

        var obj = new Obj()
        {
            RoomId = room.Id,
            Rotation = body.Rotation,
            Translation = body.Translation,
            Movable = body.Movable,
            Scale = body.Scale,
            Model = model
        };
        await _context.Objects.AddAsync(obj);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRoomObject), new { roomId = room.Id, objId = obj.Id }, new ObjDto(obj));
    }

    [HttpGet("{id:int}/objects")]
    public async Task<IActionResult> GetRoomObjects(int id)
    {
        var room = await _context.Rooms.Include(x => x.Objects).ThenInclude(o => o.Model)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (room == null)
        {
            return NotFound();
        }

        return Ok(room.Objects.Select(x => new ObjDto(x)));
    }

    [HttpGet("{roomId:int}/objects/{objId:int}")]
    public async Task<IActionResult> GetRoomObject(int roomId, int objId)
    {
        var objs = from o in _context.Objects.Include(o => o.Model)
            where o.Id == objId && o.RoomId == roomId
            select o;
        var obj = await objs.FirstOrDefaultAsync();
        if (obj == null)
        {
            return NotFound();
        }

        return Ok(new ObjDto(obj));
    }

    [HttpDelete("{roomId:int}/objects/{objId:int}")]
    public async Task<IActionResult> RemoveRoomObject(int roomId, int objId)
    {
        var objs = from o in _context.Objects
            where o.Id == objId && o.RoomId == roomId
            select o;
        var obj = await objs.FirstOrDefaultAsync();
        if (obj == null)
        {
            return NotFound();
        }

        _context.Objects.Remove(obj);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{roomId:int}/objects/{objId:int}")]
    public async Task<IActionResult> UpdateRoomObject(int roomId, int objId, [FromBody] AddRoomObjBody body)
    {
        var room = await _context.Rooms.Include(x => x.Objects).FirstOrDefaultAsync(x => x.Id == roomId);
        if (room == null)
        {
            return NotFound();
        }

        var obj = room.Objects.FirstOrDefault(x => x.Id == objId);
        if (obj == null)
        {
            return NotFound();
        }

        var model = await _context.Models.FirstOrDefaultAsync(x => x.Id == body.ModelId);
        if (model == null)
        {
            return BadRequest();
        }

        obj.RoomId = room.Id;
        obj.Rotation = body.Rotation;
        obj.Translation = body.Translation;
        obj.Movable = body.Movable;
        obj.Scale = body.Scale;
        obj.Model = model;
        
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
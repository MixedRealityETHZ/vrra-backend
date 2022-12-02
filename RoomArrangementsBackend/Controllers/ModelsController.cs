using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minio;
using RoomArrangementsBackend.Data;
using RoomArrangementsBackend.Models;

namespace RoomArrangementsBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class ModelsController : ControllerBase
{
    private readonly ILogger<ModelsController> _logger;
    private readonly DataContext _context;


    public ModelsController(ILogger<ModelsController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddModel([FromBody] AddModelBody body)
    {
        var asset = await _context.Assets.FirstOrDefaultAsync(
            a => a.Id == body.AssetId && a.Status == AssetStatus.Ready
        );
        if (asset == null)
        {
            return BadRequest();
        }

        var model = new Model()
        {
            AssetId = body.AssetId,
            Name = body.Name,
        };
        await _context.Models.AddAsync(model);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetModel), new { id = model.Id }, new ModelDto(model));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetModel(int id)
    {
        var model = await _context.Models.Include(m => m.Asset).FirstOrDefaultAsync(m => m.Id == id);

        if (model == null)
        {
            return NotFound();
        }

        return Ok(new ModelDto(model));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteModel(int id)
    {
        var model = await _context.Models.FindAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        _context.Models.Remove(model);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
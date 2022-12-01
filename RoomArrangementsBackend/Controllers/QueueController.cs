using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minio;
using RoomArrangementsBackend.Data;
using RoomArrangementsBackend.Models;

namespace RoomArrangementsBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class QueueController : ControllerBase
{
    private readonly ILogger<QueueController> _logger;
    private readonly DataContext _context;
    private readonly MinioClient _minio;
    private IConfigurationSection _minioConfig;


    public QueueController(ILogger<QueueController> logger, DataContext context, MinioClient minio,
        IConfiguration config)
    {
        _logger = logger;
        _context = context;
        _minio = minio;
        _minioConfig = config.GetSection("Minio");
    }

    [HttpPost]
    public async Task<IActionResult> Push([FromBody] PushQueueBody body)
    {
        var item = new QueueItem()
        {
            AssetId = body.AssetId,
            Name = body.Name,
        };
        await _context.Queue.AddAsync(item);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetQueueItem), new { id = item.Id });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetQueueItem(int id)
    {
        var item = await _context.Queue.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        return Ok(new QueueItemDto(item));
    }

    [HttpGet("pop")]
    public async Task<IActionResult> Pop()
    {
        QueueItem? item = null;
        await using (var trans = await _context.Database.BeginTransactionAsync())
        {
            var items = from i in _context.Queue.Include(item => item.Asset)
                where i.Status == QueueItemStatus.Pending && i.Asset.Status == AssetStatus.Ready
                orderby i.Created
                select i;
            item = items.FirstOrDefault();
            if (item == null)
            {
                return NoContent();
            }

            item.Status = QueueItemStatus.InProgress;
            item.Started = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            await trans.CommitAsync();
        }
        return Ok(new QueueItemDto(item));
    }
    
    [HttpPost("{id}/completed")]
    public async Task<IActionResult> SetCompleted(int id, [FromBody] CompleteQueueItemBody body)
    {
        var item = await _context.Queue.Include(item => item.Asset).FirstOrDefaultAsync(item => item.Id == id);
        if (item == null)
        {
            return NotFound();
        }
        
        if(item.Status != QueueItemStatus.InProgress)
        {
            return BadRequest();
        }
        
        item.Status = QueueItemStatus.Completed;
        item.Completed = DateTime.UtcNow;
        item.Message = body.Message;
        
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpPost("{id}/failed")]
    public async Task<IActionResult> SetFailed(int id, [FromBody] CompleteQueueItemBody body)
    {
        var item = await _context.Queue.Include(item => item.Asset).FirstOrDefaultAsync(item => item.Id == id);
        if (item == null)
        {
            return NotFound();
        }
        
        if(item.Status != QueueItemStatus.InProgress)
        {
            return BadRequest();
        }
        
        item.Status = QueueItemStatus.Failed;
        item.Completed = DateTime.UtcNow;
        item.Message = body.Message;
        
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

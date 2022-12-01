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
    public async Task<IActionResult> Post([FromBody] QueueItemPostBody body)
    {
        var asset = new Asset();
        await _context.Assets.AddAsync(asset);
        await _context.SaveChangesAsync();

        var args = new PresignedPutObjectArgs()
            .WithBucket(_minioConfig["Bucket"])
            .WithExpiry(_minioConfig.GetValue<int>("PutUrlExpiry"))
            .WithObject(asset.Id.ToString());

        var url = await _minio.PresignedPutObjectAsync(args);
        var item = new QueueItem()
        {
            Asset = asset,
            Name = body.Name,
        };
        await _context.Queue.AddAsync(item);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, new { UploadUrl = url });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _context.Queue.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        return Ok(item.ToDto());
    }

    [HttpPost("{id}/uploaded")]
    public async Task<IActionResult> SetUpdated(int id)
    {
        var item = await _context.Queue.Include(item => item.Asset).FirstOrDefaultAsync(item => item.Id == id);
        if (item == null)
        {
            return NotFound();
        }
        
        if(item.Asset.Status != AssetStatus.Uploading)
        {
            return BadRequest();
        }
        item.Asset.Status = AssetStatus.Ready;
        await _context.SaveChangesAsync();

        return Ok(item.ToDto());
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
        var args = new PresignedGetObjectArgs()
            .WithBucket(_minioConfig["Bucket"])
            .WithExpiry(_minioConfig.GetValue<int>("GetUrlExpiry"))
            .WithObject(item.Asset.Id.ToString());
        var url = await _minio.PresignedGetObjectAsync(args);
        return Ok(new { DownloadUrl = url, Item = item.ToDto() });
    }
    
    [HttpPost("{id}/completed")]
    public async Task<IActionResult> SetCompleted(int id, [FromBody] QueueItemCompleteBody body)
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

        return Ok(item.ToDto());
    }
    
    [HttpPost("{id}/failed")]
    public async Task<IActionResult> SetFailed(int id, [FromBody] QueueItemCompleteBody body)
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

        return Ok(item.ToDto());
    }
}

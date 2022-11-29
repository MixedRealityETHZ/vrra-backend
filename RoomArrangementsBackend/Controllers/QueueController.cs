using System.Data;
using Microsoft.AspNetCore.Mvc;
using Minio;
using RoomArrangementsBackend.Data;
using RoomArrangementsBackend.Models;

namespace RoomArrangementsBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class QueueController : ControllerBase
{
    private readonly ILogger<RoomsController> _logger;
    private readonly DataContext _context;
    private readonly MinioClient _minio;
    private string _bucketName;


    public QueueController(ILogger<RoomsController> logger, DataContext context, MinioClient minio,
        IConfiguration config)
    {
        _logger = logger;
        _context = context;
        _minio = minio;
        _bucketName = config["Minio:BucketName"] ?? throw new NoNullAllowedException();
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var asset = new Asset();
        await _context.Assets.AddAsync(asset);

        var args = new PresignedPutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(asset.Id.ToString());

        var url = await _minio.PresignedPutObjectAsync(args);
        var item = new QueueItem()
        {
            Asset = asset,
        };
        await _context.Queue.AddAsync(item);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, new { UploadUrl = url });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var queue = await _context.Queue.FindAsync(id);
        if (queue == null)
        {
            return NotFound();
        }

        return Ok(queue);
    }
}
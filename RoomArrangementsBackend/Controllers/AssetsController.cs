using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minio;
using RoomArrangementsBackend.Data;
using RoomArrangementsBackend.Models;

namespace RoomArrangementsBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class AssetsController : ControllerBase
{
    private readonly ILogger<AssetsController> _logger;
    private readonly DataContext _context;
    private readonly MinioClient _minio;
    private IConfigurationSection _minioConfig;


    public AssetsController(ILogger<AssetsController> logger, DataContext context, MinioClient minio,
        IConfiguration config)
    {
        _logger = logger;
        _context = context;
        _minio = minio;
        _minioConfig = config.GetSection("Minio");
    }

    [HttpPost]
    public async Task<IActionResult> AddAsset([FromBody] AddAssetBody body)
    {
        var asset = new Asset(){
            Name = body.Name,
            Status = AssetStatus.Uploading,
        };
        await _context.Assets.AddAsync(asset);
        await _context.SaveChangesAsync();

        var args = new PresignedPutObjectArgs()
            .WithBucket(_minioConfig["Bucket"])
            .WithExpiry(_minioConfig.GetValue<int>("PutUrlExpiry"))
            .WithObject($"{asset.Id}-{asset.Name}");

        var url = await _minio.PresignedPutObjectAsync(args);

        return CreatedAtAction(nameof(GetAsset), new { id = asset.Id }, new AssetDto(asset, url));
    }
    
    [HttpPost("{id}/uploaded")]
    public async Task<IActionResult> SetAssetUpdated(int id)
    {
        var asset = await _context.Assets.FindAsync(id);
        if (asset == null)
        {
            return NotFound();
        }
        
        if(asset.Status != AssetStatus.Uploading)
        {
            return BadRequest();
        }
        asset.Status = AssetStatus.Ready;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsset(int id)
    {
        var asset = await _context.Assets.FindAsync(id);
        if (asset == null)
        {
            return NotFound();
        }

        string? url = null;
        if (asset.Status == AssetStatus.Ready)
        {
            var args = new PresignedGetObjectArgs()
                .WithBucket(_minioConfig["Bucket"])
                .WithExpiry(_minioConfig.GetValue<int>("GetUrlExpiry"))
                .WithObject($"{asset.Id}-{asset.Name}");

            url = await _minio.PresignedGetObjectAsync(args);
        }

        return Ok(new AssetDto(asset, url));
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomArrangementsBackend.Data;

namespace RoomArrangementsBackend.Controllers
{
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
            return Ok(await _context.Rooms.ToListAsync());
        }
    }
}

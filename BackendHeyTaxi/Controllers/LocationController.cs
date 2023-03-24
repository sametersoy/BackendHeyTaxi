using MarketBackend;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace BackendHeyTaxi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<LocationController> _logger;

        public LocationController(ILogger<LocationController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetLocation")]
        public async Task<locations> GetLocation()
        {
            DataDbContext db = new DataDbContext();
            var locations= await (from q in db.locations orderby q.id descending select q).FirstOrDefaultAsync();
            return locations;
        }

        [HttpPost("AddLocation")]
        public async Task<locations> AddLocation(locations locasion)
        {
            DataDbContext db = new DataDbContext();
            locations tbl = locasion;
            db.locations.Add(locasion);
            await db.SaveChangesAsync();

            return tbl;
        }
    }
}
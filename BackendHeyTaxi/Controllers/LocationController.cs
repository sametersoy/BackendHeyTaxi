using BackendHeyTaxi.Validators;
using MarketBackend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace BackendHeyTaxi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {
      
        private readonly ILogger<LocationController> _logger;

        public LocationController(ILogger<LocationController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetLocation")]
        [Authorize]
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
            var LocationValidator = new LocationValidate();

            var result = LocationValidator.Validate(locasion);
            if (result.IsValid)
            {
               
                db.locations.Add(locasion);
                await db.SaveChangesAsync();
            }

            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();

            return tbl;
        }
    }
}
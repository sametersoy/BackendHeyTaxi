using BackendHeyTaxi.Validators;
using MarketBackend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq;
using System.Security.Claims;

namespace BackendHeyTaxi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {
      
        private readonly ILogger<LocationController> _logger;
        DataDbContext db = new DataDbContext();

        public LocationController(ILogger<LocationController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetLocation")]
        [Authorize]
        public async Task<locations> GetLocation()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
               string userId = identity.FindFirst("Id").Value;

            }
            DataDbContext db = new DataDbContext();
            var locations= await (from q in db.locations orderby q.id descending select q).FirstOrDefaultAsync();
            return locations;
        }

        [HttpGet("GetAllLocation")]
        [Authorize]
        public async Task<locations[]> GetAllLocation(string type)
        {
            int userId = 0;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                userId = Convert.ToInt32( identity.FindFirst("Id").Value);

            }
            
            var locations_type = await (from q in db.locations where q.type == (type == "Y" ? "T" : "Y") || q.userid == userId select q).ToArrayAsync();
    
        
            Console.WriteLine("GetAll Run" + " > " + locations_type.Count().ToString());
            return locations_type;
        }

        [HttpPost("AddLocation")]
        [Authorize]
        
        public async Task<locations> AddLocation(locations locasion)
        {
            string userId = "";
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                userId = identity.FindFirst("Id").Value;

            }
            DataDbContext db = new DataDbContext();
            locations tbl = locasion;

            int intUser =Convert.ToInt32(userId);
            var checkLocation =await (from q in db.locations where q.userid == intUser select q).FirstOrDefaultAsync();
            if (checkLocation != null)
            {
                //checkLocation = locasion;
                checkLocation.accuracy = locasion.accuracy;
                checkLocation.altitudeaccuracy = locasion.altitudeaccuracy;
                checkLocation.speedaccuracy = locasion.speedaccuracy;
                checkLocation.altitude = locasion.altitude;
                checkLocation.timestamp = locasion.timestamp;
                checkLocation.longitude = locasion.longitude;
                checkLocation.latitude = locasion.latitude;
                checkLocation.heading = locasion.heading;
                //checkLocation.type = locasion.type;
                checkLocation.speed = locasion.speed;
                checkLocation.floor = locasion.floor;
                checkLocation.frommockprovider = locasion.frommockprovider;
                checkLocation.userid= intUser;
                checkLocation.created_date = DateTime.UtcNow;
                checkLocation.created_by = "test2";
                checkLocation.type = locasion.type;

                await db.SaveChangesAsync();

            }
            else {
                var LocationValidator = new LocationValidate();

                var result = LocationValidator.Validate(locasion);
                if (result.IsValid)
                {
                    tbl.userid = Convert.ToInt32(userId);
                    db.locations.Add(tbl);
                    await db.SaveChangesAsync();

                }
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            }
            Console.WriteLine("new locations altitude : " + locasion.latitude.ToString() + "new locations longitude : " + locasion.longitude.ToString());



            return tbl;
        }
    }
}
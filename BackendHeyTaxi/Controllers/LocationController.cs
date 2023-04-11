using BackendHeyTaxi.Validators;
using MarketBackend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Claims;

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
        public async Task<locations[]> GetAllLocation()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                string userId = identity.FindFirst("Id").Value;

            }
            DataDbContext db = new DataDbContext();
            var locations = await (from q in db.locations orderby q.id descending select q).ToArrayAsync();

            return locations;
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
            var checkLocation = await (from q in db.locations where q.userid == intUser select q).FirstOrDefaultAsync();
            if (checkLocation != null)
            {
                checkLocation = locasion;
                checkLocation.userid= intUser;
                checkLocation.created_date = DateTime.UtcNow;
                checkLocation.created_by = "test";
            }
            else {
                var LocationValidator = new LocationValidate();

                var result = LocationValidator.Validate(locasion);
                if (result.IsValid)
                {
                    tbl.userid = Convert.ToInt32(userId);
                    db.locations.Add(tbl);
                }
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            }
            await db.SaveChangesAsync();
            Console.WriteLine("new locations : " + locasion.altitude.ToString() + locasion.longitude.ToString());



            return tbl;
        }
    }
}
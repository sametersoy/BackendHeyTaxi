using BackendHeyTaxi.Validators;
using MarketBackend;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using FluentValidation;

namespace BackendHeyTaxi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
      
        private readonly ILogger<AccountController> _logger;
        //private readonly UserManager<UserPofile> userManager;
        //private readonly SignInManager<UserPofile> signInManager;
        private readonly ILogger<AccountController> logger;
        private readonly IConfiguration configuration;

        public AccountController(IConfiguration configuration, ILogger<AccountController> logger /*IMapper mapper, UserManager<UserPofile> userManager, SignInManager<UserPofile> signInManager*/)
        {
            //this.userManager = userManager;
            //this.signInManager = signInManager;
            this.logger = logger;
            this.configuration = configuration;
        }

        [HttpGet("Login")]
        public async Task<string> Login(string username, string password)
        {
            DataDbContext db = new DataDbContext();
           


            return await GenerateToken(username, password);
        }

        [HttpPost("AddLocation")]
        public async Task<users> AddLocation(users user)
        {
            DataDbContext db = new DataDbContext();
            users tbl = user;
            db.users.Add(tbl);
            await db.SaveChangesAsync();
            return tbl;
        }

        private async Task<string> GenerateToken(string email,string password)
        {
            var securitykey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["jWTSetting:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);



            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,email),
            };
          

            var token = new JwtSecurityToken(
                issuer: configuration["jWTSetting:Issuer"],
                audience: configuration["jWTSetting:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToInt32(configuration["jWTSetting:Duration"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
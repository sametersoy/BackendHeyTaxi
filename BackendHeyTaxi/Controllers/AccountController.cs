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
        public async Task<Token> Login(string email, string password)
        {
            DataDbContext db = new DataDbContext();
            Token t = new Token();

            var checkUser = await (from q in db.users where q.email == email && q.password == password select q).FirstOrDefaultAsync();
            if (checkUser != null)
            {
                string Token = await GenerateToken(email, password);
                t.token = Token;
                return t;
            }
            else {
               throw new Exception("Girilen kullanýcý bilgileri yanlýþtýr.");
            }

        }

        [HttpPost("Register")]
        public async Task<Token> Register(users user)
        {
            DataDbContext db = new DataDbContext();

            var userCheck = await (from q in db.users where q.email == user.email select q).FirstOrDefaultAsync();
            if (userCheck != null)
            {
                throw new Exception("Bu mail adresi ile daha önce kayýt olunmuþ.");
            }
            Token t = new Token();
            var userValidator = new UserValidate();
            var result = userValidator.Validate(user);
            if (result.IsValid)
            {
                users tbl = user;
                tbl.created_date = DateTime.UtcNow;
                db.users.Add(tbl);
                await db.SaveChangesAsync();
                string Token = await GenerateToken(user.email, user.password);
                t.token = Token;
                return t;
            }
            else {
                throw new InvalidOperationException(result.Errors[0].ErrorMessage);
            }
           
        }

     

        public class Token {
            public string token { get; set; }
        }

        private async Task<string> GenerateToken(string email,string password)
        {
            var securitykey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["jWTSetting:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            IdentityOptions _options = new IdentityOptions();
            DataDbContext db = new DataDbContext();
            var user = await (from q in db.users where q.email == email select q).FirstOrDefaultAsync();


            var claims = new List<Claim>
            {
                new Claim("Id", user.id.ToString()),
                new Claim("Email", user.email.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, configuration["jWTSetting:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            };
          

            var token = new JwtSecurityToken(
                issuer: configuration["jWTSetting:Issuer"],
                audience: configuration["jWTSetting:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMonths(Convert.ToInt32(configuration["jWTSetting:Duration"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
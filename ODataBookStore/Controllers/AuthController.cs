using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ODataBookStore.Models;

namespace ODataBookStore.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BookStoreContext _context;
        private IConfiguration _configuration;
        public AuthController(BookStoreContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            var user = _context.Users.Where(x=>x.Username == login.Username && x.Password==login.Password).FirstOrDefault();
            if (user == null)
                return Unauthorized();
            TokenModel jwt = await GenerateToken(user);
            return Ok(jwt);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User register)
        {
            string error = "";
            var user = _context.Users.Where(x=>x.Username==register.Username).FirstOrDefault();
            if (user != null)
                return BadRequest(new
                {
                    error = "User already exist"
                }) ;
            _context.Users.Add(register);
            _context.SaveChanges();
            return Ok(register);
        }

        private async Task<TokenModel> GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role,user.Role.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credential = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                audience: _configuration["Jwt:Audience"],
                issuer: _configuration["Jwt:Issuer"],
                signingCredentials: credential,
                //client-side set cookies for 30 days
                expires: DateTime.UtcNow.AddDays(30)
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);  

            return new TokenModel
            {
                jwt = jwt
            };
        }
    }
}

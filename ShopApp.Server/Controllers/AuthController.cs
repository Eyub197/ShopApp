using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopApp.Core.ViewModels;
using ShopApp.Core.ViewModels.Authorization;
using ShopApp.Infrastrucutre.Configurations;
using ShopApp.Server.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private IConfiguration configuration;
        public AuthController(UserManager<User> _userManager,IConfiguration _configuration)
        {
            userManager = _userManager;
            configuration = _configuration;
            
        }
        [HttpPost("register")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<AuthViewModel>> Register([FromBody] RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthViewModel()
                {
                    Errors = new List<string>() { "Invalid payload" },
                    Result = false
                });
            }
            var existingUser = await userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest(new AuthViewModel()
                {
                    Errors = new List<string>() { "Email already in use" },
                    Result = false
                });
            }

            User user= new User()
            {
                Email = model.Email,
                UserName = model.Username
            };
            var isCreated = await userManager.CreateAsync(user, model.Password);

            if (isCreated.Succeeded)
            {
                var jwtToken = GenerateJwtToken(user);
                return Ok(new AuthViewModel()
                {
                    Result = true,
                    Token = jwtToken
                });
            }
            else
            {
                return BadRequest(new AuthViewModel()
                {
                    Errors = new List<string>() { "Unexpected error" },
                    Result = false
                });
            }
            
        }

        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AuthViewModel>> LogIn(RegistrationViewModel model)
        {
            return Ok();  
        }
        private string GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();            
            var key = Encoding.ASCII.GetBytes(configuration["JwtConfig:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MusicDistribution.API.Auth;

namespace MusicDistribution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config) => _config = config;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Hardcoded/seeded credential check
            if (request.Username != "admin" || request.Password != "admin123")
                return Unauthorized(new { message = "Invalid credentials." });

            var token = JwtTokenGenerator.GenerateToken(request.Username, _config);
            return Ok(new { token });
        }
    }
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

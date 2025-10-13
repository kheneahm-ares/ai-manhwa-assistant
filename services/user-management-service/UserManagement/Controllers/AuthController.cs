using Microsoft.AspNetCore.Mvc;
using UserManagement.API.Services.Auth;

namespace UserManagement.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private AuthService _authService { get; set; }
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO user)
        {
            var result = await _authService.RegisterUser(user);
            if (result)
                return Ok(new { Message = "User registered successfully." });
            return BadRequest(new { Message = "User registration failed." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO user)
        {
            var result = await _authService.LoginUser(user);
            if (result)
                return Ok(new { Message = "User logged in successfully." });
            return Unauthorized(new { Message = "Invalid email or password." });
        }
    }
}

using CineDigital.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineDigital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(UserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        //Request model for user registration
        public class RegisterRequest
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        //Request model for user login
        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        //Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            _logger.LogInformation($"Received registration request for email: {registerRequest.Email}");
            try
            {
                var token = await _userService.RegisterAsync(registerRequest.Name, registerRequest.Email, registerRequest.Password);
                _logger.LogInformation($"User registered successfully: {registerRequest.Email}");
                return Ok(new { message = "User created" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Authenticates a user and returns a JWT token.
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            _logger.LogInformation($"Received login request for email: {loginRequest.Email}");
            try
            {
                var token = await _userService.LoginAsync(loginRequest.Email, loginRequest.Password);
                _logger.LogInformation($"User authenticated successfully: {loginRequest.Email}");
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
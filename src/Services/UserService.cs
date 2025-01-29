using CineDigital.Context;
using CineDigital.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using dotenv.net;

namespace CineDigital.Services
{

    public class UserService
    {
        private readonly CineDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(CineDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;

            DotEnv.Load();
        }

        //Registers a new user and generates a JWT token.
        public async Task<string> RegisterAsync(string name, string email, string password)
        {
            _logger.LogInformation($"Starting registration for user: {email}");

            if (_context.Users.Any(u => u.Email == email))
                throw new Exception("User already exists.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User registered successfully: {Email}", email);
            return GenerateJwtToken(user);
        }

        //Authenticates a user and generates a JWT token.
        public async Task<string> LoginAsync(string email, string password)
        {
            _logger.LogInformation($"User login attempt: {email}");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Invalid credentials.");

            _logger.LogInformation($"User authenticated successfully: {email}");
            return GenerateJwtToken(user);
        }

        //Generates a JWT token for the specified user.
        private string GenerateJwtToken(User user)
        {
            _logger.LogInformation("Generating JWT token for user: {Email}", user.Email);

            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

            if (string.IsNullOrEmpty(secretKey))
                throw new Exception("JWT_SECRET_KEY is not set in the environment variables.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            _logger.LogInformation("JWT token generated for user: {Email}", user.Email);
            return tokenString;
        }
    }
}

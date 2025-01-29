using CineDigital.Models;
using CineDigital.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineDigital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowtimesController : ControllerBase
    {
        private readonly ShowtimeService _showtimeService;
        private readonly ILogger<ShowtimesController> _logger;

        public ShowtimesController(ShowtimeService showtimeService, ILogger<ShowtimesController> logger)
        {
            _showtimeService = showtimeService;
            _logger = logger;
        }

        //Retrieves all showtimes
        [HttpGet("get")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllShowtimes()
        {
            _logger.LogInformation("Fetching all showtimes.");
            var showtimes = await _showtimeService.GetAllShowtimeAsync();
            return Ok(showtimes);
        }

        //Retrieves a showtime by its ID
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShowtimeById(Guid id)
        {
            _logger.LogInformation($"Fetching showtime with Id: {id}");
            var showtime = await _showtimeService.GetShowtimeByIdAsync(id);
            if (showtime == null)
                return NotFound(new { message = "Showtime not found" });
            return Ok(showtime);
        }

        //Adds a new showtime to the database. Requires Admin role
        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddShowtime([FromBody] Showtime showtime)
        {
            _logger.LogInformation($"Adding new showtime for Movie Id: {showtime.MovieId} at Theater Id: {showtime.TheaterId}");
            try
            {
                var showtimeId = await _showtimeService.AddShowtimeAsync(showtime);
                _logger.LogInformation($"Showtime added successfully with Id: {showtime.Id}");
                return CreatedAtAction(nameof(GetShowtimeById), new { id = showtimeId }, null);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
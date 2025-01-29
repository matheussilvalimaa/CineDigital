using CineDigital.Models;
using CineDigital.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineDigital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TheatersController : ControllerBase
    {
        private readonly TheaterService _theaterService;
        private readonly ILogger<TheatersController> _logger;

        public TheatersController(TheaterService theaterService, ILogger<TheatersController> logger)
        {
            _theaterService = theaterService;
            _logger = logger;
        }

        //Retrieves all theaters
        [HttpGet("get")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTheaters()
        {
            _logger.LogInformation("Fetching all theaters");
            var theaters = await _theaterService.GetAllTheatersAsync();
            return Ok(theaters);
        }

        //Retrieves a theater by its ID
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTheaterById(Guid id)
        {
            _logger.LogInformation($"Fetching theater with Id: {id}");
            var theater = await _theaterService.GetTheaterByIdAsync(id);
            if (theater == null)
                return NotFound(new { message = "Theater not found" });
            return Ok(theater);
        }

        //Adds a new theater to the database. Requires Admin role
        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTheater([FromBody] Theater theater)
        {
            _logger.LogInformation($"Adding new theater: {theater.Name}");
            try
            {
                var theaterId = await _theaterService.AddTheaterAsync(theater);
                _logger.LogInformation($"Theater added successfully with Id: {theater.Id}");
                return CreatedAtAction(nameof(GetTheaterById), new { id = theaterId }, null);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
    }
}
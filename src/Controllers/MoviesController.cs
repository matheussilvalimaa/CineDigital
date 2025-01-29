using CineDigital.Models;
using CineDigital.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineDigital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieService _movieService;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(MovieService movieService, ILogger<MoviesController> logger)
        {
            _movieService = movieService;
            _logger = logger;
        }

        //Retrieves all movies
        [HttpGet("movies")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMovies()
        {
            _logger.LogInformation("Fetching all movies.");
            var movies = await _movieService.GetAllMoviesAsync();
            return Ok(movies);
        }

        //Retrieves a movie by its ID
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMovieById(Guid id)
        {
            _logger.LogInformation($"Fetching movie with Id: {id}");
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
                return NotFound(new { message = "Movie not found"} );
            return Ok(movie);
        }

        //Adds a new movie to the database. Requires Admin role.
        [HttpPost("addMovie")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddMovie([FromBody] Movie movie)
        {
            _logger.LogInformation($"Adding new movie: {movie.Title}");
            try
            {
                var movieId = await _movieService.AddMovieAsync(movie);
                _logger.LogInformation($"Movie successfully added with Id: {movieId}");
                return CreatedAtAction(nameof(GetMovieById), new { id = movieId }, null);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Searches for movies based on provided filters
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchMovies([FromQuery] string title, [FromQuery] string genre, [FromQuery] DateTime? date)
        {
            _logger.LogInformation("Searching movies with Title: {Title}, Genre: {Genre}, Date: {Date}", title, genre, date);
            var movies = await _movieService.SearchMoviesAsync(title, genre, date);
            return Ok(movies);
        }
    }
}
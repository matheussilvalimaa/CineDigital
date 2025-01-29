using CineDigital.Context;
using CineDigital.Models;
using Microsoft.EntityFrameworkCore;

namespace CineDigital.Services
{
    public class MovieService
    {
        private readonly CineDbContext _context;
        private readonly ILogger<MovieService> _logger;

        public MovieService(CineDbContext context, ILogger<MovieService> logger)
        {
            _context = context;
            _logger = logger;
        }

        //Retrieves all movies
        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            _logger.LogInformation("Fetching all movies!");
            return await _context.Movies.ToListAsync();
        }

        //Retrieves a movie by its ID
        public async Task<Movie> GetMovieByIdAsync(Guid id)
        {
            _logger.LogInformation($"Fetching movie with ID: {id}");
            return await _context.Movies.FindAsync(id);        
        }

        //Adds a new movie to the database
        public async Task<Guid> AddMovieAsync(Movie movie)
        {
            _logger.LogInformation($"Adding new movie: {movie.Title}");
            movie.Id = Guid.NewGuid();
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Movie added with ID: {movie.Id}");
            return movie.Id;
        }

        //Searches for movies based on provided filters
        public async Task<IEnumerable<Movie>> SearchMoviesAsync(string title, string genre, DateTime? date)
        {
            _logger.LogInformation($"Searching movies with title: {title}, genre: {genre} and date: {date}");
            var query = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(m => m.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(genre))
                query = query.Where(m => m.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase));
            
            if (date.HasValue)
                query = query.Where(m => m.ReleaseDate.Date == date.Value.Date);
            
            var result = await query.ToListAsync();
            _logger.LogInformation($"Found {result.Count} movies matching the criteria");
            return result;
        }

    }
}
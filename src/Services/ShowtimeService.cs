using CineDigital.Context;
using CineDigital.Models;
using Microsoft.EntityFrameworkCore;

namespace CineDigital.Services
{
    public class ShowtimeService
    {
        private readonly CineDbContext _context;
        private readonly ILogger<ShowtimeService> _logger;
        public ShowtimeService(CineDbContext context, ILogger<ShowtimeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        //Retrieves all showtimes with related movie and theater information
        public async Task<IEnumerable<Showtime>> GetAllShowtimeAsync()
        {
            _logger.LogInformation("Fetching all showtimes.");
            return await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Theater)
                .ToListAsync();
        }

        //Retrieves a showtime by its ID with related movie and theater information.
        public async Task<Showtime> GetShowtimeByIdAsync(Guid id)
        {
            _logger.LogInformation($"Fetching showtime with ID: {id}");
            return await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Theater)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        //Adds a new showtime to the database
        public async Task<Guid> AddShowtimeAsync(Showtime showtime)
        {
             _logger.LogInformation($"Adding new showtime for Movie ID: {showtime.MovieId} at Theater ID: {showtime.TheaterId}");
            showtime.Id = Guid.NewGuid();
            _context.Showtimes.Add(showtime);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Showtime added with ID: {showtime.Id}");
            return showtime.Id;
        }
    }
}
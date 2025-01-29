using CineDigital.Context;
using CineDigital.Models;
using Microsoft.EntityFrameworkCore;

namespace CineDigital.Services
{
    public class TheaterService
    {
        private readonly CineDbContext _context;
        private readonly ILogger<TheaterService> _logger;

        public TheaterService(CineDbContext context, ILogger<TheaterService> logger)
        {
            _context = context;
            _logger = logger;
        }

        //Retrieves all theaters
        public async Task<IEnumerable<Theater>> GetAllTheatersAsync()
        {
            _logger.LogInformation("Fetching all theaters");
            return await _context.Theaters.ToListAsync();
        }

        //Retrieves a theater by its Id
        public async Task<Theater> GetTheaterByIdAsync(Guid id)
        {
            _logger.LogInformation($"Fetching theater with Id: {id}");
            return await _context.Theaters.FindAsync(id);
        }

        //Adds a new theater to the database
        public async Task<Guid> AddTheaterAsync(Theater theater)
        {
            _logger.LogInformation($"Adding new theater: {theater.Name}");
            theater.Id = Guid.NewGuid();
            _context.Theaters.Add(theater);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Theater added with ID: {theater.Id}");
            return theater.Id;
        }
        
    }
}
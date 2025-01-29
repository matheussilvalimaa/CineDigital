using System.Security.Claims;
using CineDigital.Models;
using CineDigital.Services;
using CineDigital.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineDigital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationService _reservationService;
        private readonly ILogger<ReservationsController> _logger;
        private readonly CineDbContext _context;

        public ReservationsController(ReservationService reservationService, ILogger<ReservationsController> logger, CineDbContext context)
        {
            _reservationService = reservationService;
            _logger = logger;
            _context = context;
        }

        //Request model for creating a reservation.
        public class ReserveRequest
        {
            public Guid ShowtimeId { get; set; }
            public Guid SeatId { get; set; }
            public string PaymentToken { get; set; }
        }

        //Creates a new reservation
        [HttpPost("create")]
        public async Task<IActionResult> Reserve([FromBody] ReserveRequest reserveRequest)
        {
            _logger.LogInformation($"Received reservation request for showtime Id: {reserveRequest.ShowtimeId} and Seat Id: {reserveRequest.SeatId}");
            try
            {
                //Get the userId from the JWT token
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var reservation = await _reservationService.ReserveSeatAsync(userId, reserveRequest.ShowtimeId, reserveRequest.SeatId, reserveRequest.PaymentToken);
                _logger.LogInformation($"Reservation created successfully with Id: {reservation.Id}");
                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Retrieves all reservations for the authenticated user.
        [HttpGet("get")]
        public async Task<IActionResult> GetUserReservations()
        {
            _logger.LogInformation("Fetching reservations for the authenticated user.");
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var reservations = await _context.Reservations
                    .Include(r => r.Showtime)
                        .ThenInclude(s => s.Movie)
                    .Include(r => r.Showtime)
                        .ThenInclude(s => s.Theater)
                    .Include(r => r.Seat)
                    .Include(r => r.Payment)
                    .Where(r => r.UserId == userId)
                    .ToListAsync();

                _logger.LogInformation($"Found {reservations.Count} reservations for user Id: {userId}");
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Get a specific reservation by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(Guid id)
        {
            _logger.LogInformation($"Fetching reservation with Id: {id}");
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var reservation = await _context.Reservations
                    .Include(r => r.Showtime)
                        .ThenInclude(s => s.Movie)
                    .Include(r => r.Showtime)
                        .ThenInclude(s => s.Theater)
                    .Include(r => r.Seat)
                    .Include(r => r.Payment)
                    .FirstOrDefaultAsync(r => r.Id == id);

                    if (reservation == null)
                        return NotFound(new { message = "Reservation not found "});
                    
                    //Verify if the user is the owner of the reservation or an Admin
                    var userRole = User.FindFirstValue(ClaimTypes.Role);
                    if (reservation.UserId != userId && userRole != "Admin")
                    {
                        _logger.LogWarning($"User Id: {userId} attemped to access reservation Id: {reservation.Id}");
                        return Forbid();
                    }

                    return Ok(reservation);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Cancels a reservation by Id
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelReservation(Guid id)
        {
            _logger.LogInformation($"Cancelation request for reservation Id: {id}");
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                var reservation = await _context.Reservations
                    .Include(r => r.Payment)
                    .FirstOrDefaultAsync(r => r.Id == id);
                
                if (reservation == null)
                    return NotFound(new { message = "Reservation not found"} );
                
                if (reservation.UserId != userId && userRole != "Admin")
                {
                    _logger.LogWarning("User ID: {UserId} attempted to cancel reservation ID: {Id} without permission.", userId, id);
                    return Forbid();
                }

                //Remove reservation and payment
                _context.Reservations.Remove(reservation);
                _context.Payments.Remove(reservation.Payment);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Reservation canceled successfully"} );
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message} );
            }
        }
    }
}
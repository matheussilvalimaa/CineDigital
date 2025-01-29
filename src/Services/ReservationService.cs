using CineDigital.Context;
using CineDigital.Models;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace CineDigital.Services
{
    public class ReservationService
    {
        private readonly CineDbContext _context;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(CineDbContext context, ILogger<ReservationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        //Creates a new reservation by reserving a seat and processing payment
        public async Task<Reservation> ReserveSeatAsync(Guid userId, Guid showtimeId, Guid seatId, string paymentToken)
        {
            _logger.LogInformation($"User Id: {userId} attemping to reserve Seat Id: {seatId} for Showtime Id: {showtimeId}");
            
            //Verify if the seat is already reserved
            bool isReserved = await _context.Reservations
                .AnyAsync(r => r.ShowtimeId == showtimeId && r.SeatId == seatId);
            if (isReserved)
            {
                _logger.LogWarning($"Seat Id: {seatId}, for showtimeId: {showtimeId} is already reserved");
                throw new Exception("Seat is already reserved");
            }

            //Process payment with Stripe
            var chargeOptions = new ChargeCreateOptions
            {
                Amount = 2000,
                Currency = "BRL",
                Description = "Seat Reservation",
                Source = paymentToken,
            };

            var chargeService = new ChargeService();
            Charge charge = await chargeService.CreateAsync(chargeOptions);

            _logger.LogInformation($"Payment processed with Status: {charge.Status} and Charge Id: {charge.Id}");

            if (charge.Status != "succeeded")
                _logger.LogError($"Payment failed for Charge Id: {charge.Id}");

            //Create reservation and payment inside a transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var reservation = new Reservation
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    ShowtimeId = showtimeId,
                    SeatId = seatId,
                    ReservationTime = DateTime.UtcNow,
                    Payment = new Payment
                    {
                        Id = Guid.NewGuid(),
                        PaymentId = charge.Id,
                        Amount = charge.Amount / 100m,
                        PaymentDate = DateTime.UtcNow,
                        Status = charge.Status
                    }
                };

                _context.Reservations.Add(reservation);
                _context.Payments.Add(reservation.Payment);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                _logger.LogInformation($"Reservation created with Id: {reservation.Id}");
                return reservation;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error ocurred while creating reservation");
                throw;
            }

        }
    }
}
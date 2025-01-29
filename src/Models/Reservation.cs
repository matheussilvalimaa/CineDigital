namespace CineDigital.Models
{
    public class Reservation
    {
        public Guid Id { get; set;}
        public Guid UserId { get; set;}
        public User User { get; set;}
        public Showtime Showtime { get; set;}
        public Guid ShowtimeId { get; set;}
        public Seat Seat { get; set;}
        public Guid SeatId { get; set;}
        public DateTime ReservationTime {get; set;}
        public Payment Payment { get; set;}

    }
}
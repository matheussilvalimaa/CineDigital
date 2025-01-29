namespace CineDigital.Models
{
    public class Seat
    {
        public Guid Id { get; set;}
        public string SeatNumber { get; set;}
        public string Row { get; set;}
        public Guid TheaterId { get; set;}
        public Theater Theater { get; set;}
        public ICollection<Reservation> Reservations { get; set;}
    }
}
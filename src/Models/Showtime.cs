namespace CineDigital.Models
{
    public class Showtime
    {
        public Guid Id { get; set;}
        public Guid MovieId { get; set;}
        public Movie Movie { get; set;}
        public Guid TheaterId { get; set;}
        public Theater Theater { get; set;}
        public DateTime StartTime { get; set;}
        public DateTime EndTime { get; set;}
        public ICollection<Reservation> Reservations { get; set;}
    }
}
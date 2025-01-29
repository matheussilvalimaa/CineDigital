namespace CineDigital.Models
{
    public class Theater
    {
        public Guid Id { get; set;}
        public string Name { get; set;}
        public string Location { get; set;}
        public int TotalSeats { get; set;}
        public ICollection<Seat> Seats { get; set;}
        public ICollection<Showtime> Showtimes { get; set;}
    }
}
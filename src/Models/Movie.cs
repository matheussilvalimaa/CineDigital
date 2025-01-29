namespace CineDigital.Models
{
    public class Movie
    {
        public Guid Id { get; set;}
        public string Title { get; set;}
        public string Description { get; set;}
        public string Genre { get; set;}
        public int Duration { get; set;}
        public DateTime ReleaseDate { get; set;}
        public ICollection<Showtime> Showtimes { get; set;}

    }
}
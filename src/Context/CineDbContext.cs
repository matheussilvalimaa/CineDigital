using Microsoft.EntityFrameworkCore;
using CineDigital.Models;

namespace CineDigital.Context
{
    public class CineDbContext : DbContext
    {
        public CineDbContext(DbContextOptions<CineDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set;}
        public DbSet<Movie> Movies { get; set;}
        public DbSet<Theater> Theaters { get; set;}
        public DbSet<Showtime> Showtimes { get; set;}
        public DbSet<Seat> Seats { get; set;}
        public DbSet<Reservation> Reservations { get; set;}
        public DbSet<Payment> Payments { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Payment)
                .WithOne(p => p.Reservation)
                .HasForeignKey<Payment>(p => p.ReservationId);
        }

    }
}
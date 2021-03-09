using Microsoft.EntityFrameworkCore;
using Cinema.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Cinema.Data
{
    public class CinemaContext : DbContext
    {
        public DbSet<AgeRating> AgeRatings { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Postcode> Postcodes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieSchedule> MovieSchedules { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<SeatLocation> SeatLocations { get; set; }
        public DbSet<Crew> Crews { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        public DbSet<Role> Roles { get; set; }

        // Many to many tables
        public DbSet<MovieCrew> MovieCrew { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }

        //public CinemaContext(DbContextOptions<CinemaContext> options)
        //    : base(options)
        //{ }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(ConsoleLoggerFactory)
                .EnableSensitiveDataLogging()
                .UseSqlServer(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = H3Cinema");
        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieGenre>().HasKey(x => new { x.MovieId, x.GenreId });
            modelBuilder.Entity<MovieCrew>().HasKey(x => new { x.MovieId, x.RoleId, x.CrewId });
            modelBuilder.Entity<Seat>().HasIndex(x => new { x.SeatLocationId, x.MovieScheduleId }).IsUnique();
            modelBuilder.Entity<Booking>().HasIndex(x => new { x.SeatId, x.MovieScheduleId }).IsUnique();
            modelBuilder.Entity<Postcode>().HasIndex(x => new { x.Code }).IsUnique();
            //modelBuilder.Entity<BookingSeat>().HasKey(x => new { x.BookingId, x.SeatId });
        }

        public static readonly ILoggerFactory ConsoleLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name &&
                level == LogLevel.Information)
            .AddConsole();
        });
    }
}
﻿using Cinema.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cinema.Data
{
    public class CinemaContext : DbContext
    {
        // Database Class

        // Tables
        public DbSet<User> Users { get; set; }
        public DbSet<AgeRating> AgeRatings { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Postcode> Postcodes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Screening> Screenings { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<SeatLocation> SeatLocations { get; set; }
        public DbSet<Crew> Crews { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        public DbSet<Role> Roles { get; set; }

        // Many to many tables
        public DbSet<MovieCrew> MovieCrew { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public CinemaContext()
        { }

        public CinemaContext(DbContextOptions<CinemaContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = H3Cinema;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add Contrains to the Database

            // Primary Keys
            modelBuilder.Entity<MovieGenre>().HasKey(x => new { x.MovieId, x.GenreId });
            modelBuilder.Entity<MovieCrew>().HasKey(x => new { x.MovieId, x.RoleId, x.CrewId });

            // Set Unique on specified colum/colums
            modelBuilder.Entity<Seat>().HasIndex(x => new { x.SeatLocationId, x.ScreeningId }).IsUnique();
            modelBuilder.Entity<Booking>().HasIndex(x => new { x.SeatId }).IsUnique();
            modelBuilder.Entity<Postcode>().HasIndex(x => new { x.Code }).IsUnique();
            modelBuilder.Entity<AgeRating>().HasIndex(x => new { x.RatingName }).IsUnique();
            modelBuilder.Entity<Genre>().HasIndex(x => new { x.Name }).IsUnique();
        }

        // Logging, used in console app.
        public static readonly ILoggerFactory ConsoleLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name &&
                level == LogLevel.Information)
            .AddConsole();
        });
    }
}
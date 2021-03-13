using Cinema.Data;
using Cinema.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    class SeedRelations
    {
        private CinemaContext _context;
        public SeedRelations(CinemaContext context)
        {
            _context = context;
        }


        public void PopulateDatabaseRelation()
        {

            _context.AddRange(PopulateMovieGenre());
            _context.AddRange(PopulateMovieCrew());
            _context.AddRange(PopulateScreeningsNSeats());
            _context.UpdateRange(PopulateCustomerPostcode());
            _context.UpdateRange(PopulateMovieAgeRating());
            _context.SaveChanges();

            _context.AddRange(PopulateBookings());
            _context.SaveChanges();
        }

        private List<MovieGenre> PopulateMovieGenre()
        {
            Random rnd = new Random();
            List<MovieGenre> movieGenres = _context.MovieGenres.ToList();
            List<Movie> movies = _context.Movies.ToList();
            List<Genre> genres = _context.Genres.ToList();

            foreach (var movie in movies)
            {
                List<Genre> randomGenres = genres.OrderBy(x => rnd.Next()).Take(rnd.Next(1, 4)).ToList();
                foreach (var randomGenre in randomGenres)
                {
                    movieGenres.Add(new MovieGenre() { Movie = movie, Genre = randomGenre });
                }
            }

            return movieGenres;
        }

        private List<MovieCrew> PopulateMovieCrew()
        {
            Random rnd = new Random();
            List<MovieCrew> movieCrews = _context.MovieCrew.ToList();
            List<Movie> movies = _context.Movies.ToList();
            List<Crew> crew = _context.Crews.ToList();
            List<Role> roles = _context.Roles.ToList();

            foreach (var movie in movies)
            {
                List<Crew> randomCrews = crew.OrderBy(x => rnd.Next()).Take(rnd.Next(3, 10)).ToList();

                foreach (var randomCrew in randomCrews)
                {
                    Role randomRole = roles.OrderBy(x => rnd.Next()).First();
                    movieCrews.Add(new MovieCrew() { Movie = movie, Crew = randomCrew, Role = randomRole });
                }

            }

            return movieCrews;
        }

        private List<Customer> PopulateCustomerPostcode()
        {
            List<Customer> customers = _context.Customers.ToList();
            List<Postcode> postcodes = _context.Postcodes.ToList();
            var rnd = new Random();

            foreach (var customer in customers)
            {
                customer.Postcode = postcodes.OrderBy(x => rnd.Next()).FirstOrDefault();
            }

            return customers;
        }

        private List<Movie> PopulateMovieAgeRating()
        {
            List<Movie> movies = _context.Movies.ToList();
            List<AgeRating> ageRatings = _context.AgeRatings.ToList();
            var rnd = new Random();

            foreach (var movie in movies)
            {
                movie.AgeRating = ageRatings.OrderBy(x => rnd.Next()).FirstOrDefault();
            }

            return movies;
        }

        private DateTime RandomTime()
        {
            var start = DateTime.Today;
            var rnd = new Random();
            DateTime result = start.AddDays(rnd.Next(1, 30)).AddHours(rnd.Next(8, 24)).AddMinutes(rnd.Next(5, 55));
            return result;
        }

        private List<Seat> GenerateSeatTheater(Theater theater)
        {
            var seatLocations = _context.SeatLocations.ToList();
            var theaterSeats = new List<Seat>();
            int numberOfSeats = theater.Row * theater.SeatNumber;

            seatLocations = seatLocations.Where(x => x.Row <= theater.Row && x.SeatNumber <= theater.SeatNumber)
                .OrderBy(x => x.Row).ThenBy(x => x.SeatNumber).ToList();

            for (int i = 0; i < numberOfSeats; i++)
            {
                theaterSeats.Add(new Seat() { SeatLocation = seatLocations[i] });
            }

            return theaterSeats;
        }

        private List<Screening> PopulateScreeningsNSeats()
        {
            List<Screening> Screenings = _context.Screenings.ToList();
            List<Movie> movies = _context.Movies.ToList();
            List<Theater> theaters = _context.Theaters.ToList();
            Random rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                int theaterID = rnd.Next(0, 4);
                Screenings.Add(new Screening()
                {
                    Movie = movies[i],
                    Theater = theaters[theaterID],
                    Time = RandomTime(),
                    Seats = GenerateSeatTheater(theaters[theaterID])
                });
            }

            return Screenings;
        }

        private List<Booking> PopulateBookings()
        {
            List<Booking> bookings = _context.Bookings.ToList();
            List<Customer> customers = _context.Customers.ToList();
            List<Seat> seats = _context.Seats.ToList();

            for (int i = 0; i < customers.Count(); i++)
            {
                bookings.Add(new Booking()
                {
                    Customer = customers[i],
                    Seat = seats[i]
                });
            }

            return bookings;
        }
    }
}

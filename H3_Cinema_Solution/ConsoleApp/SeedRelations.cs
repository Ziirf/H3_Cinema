using Cinema.Data;
using Cinema.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    class SeedRelations
    {
        private readonly CinemaContext _context;
        private readonly Random _random;
        public SeedRelations(CinemaContext context)
        {
            _context = context;
            _random = new Random();
        }


        public void PopulateDatabaseRelation()
        {
            // Populates all tables with relations and data
            _context.AddRange(PopulateMovieGenre());
            _context.AddRange(PopulateMovieCrew());
            _context.AddRange(PopulateScreenings(100, 20));
            _context.UpdateRange(PopulateCustomerPostcode());
            _context.UpdateRange(PopulateMovieAgeRating());
            _context.SaveChanges();

            _context.AddRange(PopulateBookings());
            _context.SaveChanges();
        }

        private List<MovieGenre> PopulateMovieGenre()
        {
            List<MovieGenre> movieGenres = _context.MovieGenres.ToList();
            List<Movie> movies = _context.Movies.ToList();
            List<Genre> genres = _context.Genres.ToList();

            // Add random Genres to movies
            foreach (var movie in movies)
            {
                List<Genre> randomGenres = genres.OrderBy(x => _random.Next()).Take(_random.Next(1, 4)).ToList();
                foreach (var randomGenre in randomGenres)
                {
                    movieGenres.Add(new MovieGenre() { Movie = movie, Genre = randomGenre });
                }
            }

            return movieGenres;
        }

        //private List<MovieCrew> PopulateMovieCrewOld()
        //{
        //    List<MovieCrew> movieCrews = _context.MovieCrew.ToList();
        //    List<Movie> movies = _context.Movies.ToList();
        //    List<Crew> crew = _context.Crews.ToList();
        //    List<Role> roles = _context.Roles.ToList();

        //    foreach (var movie in movies)
        //    {
        //        List<Crew> randomCrews = crew.OrderBy(x => _random.Next()).Take(_random.Next(3, 10)).ToList();

        //        foreach (var randomCrew in randomCrews)
        //        {
        //            Role randomRole = roles.OrderBy(x => _random.Next()).First();
        //            movieCrews.Add(new MovieCrew() { Movie = movie, Crew = randomCrew, Role = randomRole });
        //        }

        //    }

        //    return movieCrews;
        //}

        private List<MovieCrew> PopulateMovieCrew()
        {
            List<MovieCrew> movieCrews = _context.MovieCrew.ToList();
            List<Movie> movies = _context.Movies.ToList();
            List<Crew> crews = _context.Crews.ToList();
            List<Role> roles = _context.Roles.ToList();

            // Add random movies roles to movies crew
            foreach (var movie in movies)
            {
                List<Crew> rngCrews = crews.OrderBy(x => _random.Next()).Take(_random.Next(5, 10)).ToList();

                movieCrews.Add(new MovieCrew() { Movie = movie, Crew = rngCrews[0], Role = roles[0] });
                movieCrews.Add(new MovieCrew() { Movie = movie, Crew = rngCrews[1], Role = roles[1] });
                movieCrews.Add(new MovieCrew() { Movie = movie, Crew = rngCrews[2], Role = roles[2] });

                foreach (var rngCrew in rngCrews.Skip(3))
                {
                    movieCrews.Add(new MovieCrew() { Movie = movie, Crew = rngCrew, Role = roles[3] });
                }
            }

            return movieCrews;
        }

        private List<Customer> PopulateCustomerPostcode()
        {
            List<Customer> customers = _context.Customers.ToList();
            List<Postcode> postcodes = _context.Postcodes.ToList();
            var rnd = new Random();
            
            // Add random postcodes to customers
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

            // Add random Agerating to Movies
            foreach (var movie in movies)
            {
                movie.AgeRating = ageRatings.OrderBy(x => _random.Next()).FirstOrDefault();
            }

            return movies;
        }

        private List<Seat> GenerateSeatTheater(Theater theater)
        {
            var seatLocations = _context.SeatLocations.ToList();
            var theaterSeats = new List<Seat>();
            int numberOfSeats = theater.Row * theater.SeatNumber;

            // Get list of seats for a specific theater
            seatLocations = seatLocations.Where(x => x.Row <= theater.Row && x.SeatNumber <= theater.SeatNumber)
                .OrderBy(x => x.Row).ThenBy(x => x.SeatNumber).ToList();

            // Add seats to a theater
            for (int i = 0; i < numberOfSeats; i++)
            {
                theaterSeats.Add(new Seat() { SeatLocation = seatLocations[i] });
            }

            return theaterSeats;
        }

        private List<Screening> PopulateScreenings(int amountGenerated, int movieAmount = 100)
        {
            List<Screening> Screenings = _context.Screenings.ToList();
            List<Movie> movies = _context.Movies.Take(movieAmount).ToList();
            List<Theater> theaters = _context.Theaters.ToList();

            //Generate Screenings on random Theaters from random Movies
            for (int i = 0; i < amountGenerated; i++)
            {
                int theaterID = _random.Next(theaters.Count);
                Screenings.Add(new Screening()
                {
                    Movie = movies[_random.Next(movies.Count)],
                    Theater = theaters[theaterID],
                    Time = RandomTime(14),
                    Seats = GenerateSeatTheater(theaters[theaterID])
                });
            }

            return Screenings;
        }

        private DateTime RandomTime(int fromNow = 7)
        {
            //Generate RandomTime for Screening
            int[] numbArray = {0, 10, 15, 20, 30, 40, 45, 50};

            return DateTime.Today
                .AddDays(_random.Next(1, fromNow))
                .AddHours(_random.Next(10, 22))
                .AddMinutes(numbArray[_random.Next(numbArray.Length)]);
        }

        private List<Booking> PopulateBookings(int amountBooked = 100)
        {
            //Generate Bookings with random Customers with random Seats

            List<Booking> bookings = _context.Bookings.ToList();
            List<Customer> customers = _context.Customers.ToList();
            List<Seat> seats = _context.Seats.ToList();
            List<Seat> randomSeats = seats.OrderBy(x => _random.Next()).Take(amountBooked).ToList();

            foreach (var seat in randomSeats)
            {
                bookings.Add(new Booking()
                {
                    Customer = customers[_random.Next(customers.Count())],
                    Seat = seat
                });
            }

            //for (int i = 0; i < amountBooked; i++)
            //{
            //    bookings.Add(new Booking()
            //    {
            //        Customer = customers[i],
            //    });
            //}

            return bookings;
        }
    }
}

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
            //_context.AddRange(GenerateTheater());
            _context.AddRange(PopulateMovieGenre());
            _context.AddRange(PopulateMovieCrew());
            PopulateCustomerPostcode();
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

        private void PopulateCustomerPostcode()
        {
            List<Customer> customers = _context.Customers.ToList();
            List<Postcode> postcodes = _context.Postcodes.ToList();
            var rnd = new Random();

            foreach (var customer in customers)
            {
                customer.Postcode = postcodes.OrderBy(x => rnd.Next()).FirstOrDefault();
            }
        }

        //private List<Theater> GenerateTheater()
        //{
        //    List<SeatLocation> seatLocationList = _context.SeatLocations.ToList();

        //    var theaters = new List<Theater>
        //    {
        //        new Theater() { TheaterName = "MovieZilla", Seats = GenerateSeats(seatLocationList, 15, 20) },
        //        new Theater() { TheaterName = "Wax on, wax off", Seats = GenerateSeats(seatLocationList, 10, 20) },
        //        new Theater() { TheaterName = "Yippee ki-yay", Seats = GenerateSeats(seatLocationList, 10, 10) },
        //        new Theater() { TheaterName = "To infinity and beyond!", Seats = GenerateSeats(seatLocationList, 5, 10) }
        //    };

        //    return theaters;
        //}

        //private List<Seat> GenerateSeats(List<SeatLocation> seatLocations, int rows, int seats)
        //{
        //    var result = seatLocations.Where(x => x.Seat <= seats && x.Row <= rows).ToList();

        //    return result.Select(sl => new Seat() { SeatLocation = sl }).ToList();
        //}
    }
}

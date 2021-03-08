using Cinema.Data;
using Cinema.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

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
            _context.UpdateRange(AddSeatsToTheater());
            _context.AddRange(PopulateMovieSchedules());
            PopulateCustomerPostcode();
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

        private  List<Seat> GenerateSeats(List<SeatLocation> seatLocationListList, int rows, int seats)
        {

            var result = seatLocationListList.Where(x => x.Seat <= seats && x.Row <= rows).ToList();

            return result.Select(sl => new Seat() { SeatLocation = sl }).ToList();
        }

        private  List<Theater> AddSeatsToTheater()
        {

            var theaterList = _context.Theaters.ToList();
            var seatlocationlist = _context.SeatLocations.ToList();

            _context.Theaters.Find(theaterList[0].Id).Seats = GenerateSeats(seatlocationlist, 15, 20);
            _context.Theaters.Find(theaterList[1].Id).Seats = GenerateSeats(seatlocationlist, 10, 20);
            _context.Theaters.Find(theaterList[2].Id).Seats = GenerateSeats(seatlocationlist, 10, 10);
            _context.Theaters.Find(theaterList[3].Id).Seats = GenerateSeats(seatlocationlist, 5, 10);


            return theaterList;
        }
       

        private DateTime RandomTime()
        {
            var start = DateTime.Today;
            var rnd = new Random();
            DateTime result = start.AddDays(rnd.Next(1,30)).AddHours(rnd.Next(8, 24)).AddMinutes(rnd.Next(5,55));
            return result;
        }

        private List<MovieSchedule> PopulateMovieSchedules()
        {
            var movieSchedule = _context.MovieSchedules.ToList();
            var movie = _context.Movies.ToList();
            var theater = _context.Theaters.ToList();
            var rnd = new Random();

            for (int i = 0; i < 50; i++)
            {
                movieSchedule.Add(new MovieSchedule() { Movie = movie[i], Theater = theater[rnd.Next(0, 4)], Time = RandomTime()});
            }


            return movieSchedule;
        }

        private List<Booking> PopulateBookings()
        {
            var booking = _context.Bookings.ToList();
            var customer = _context.Customers.ToList();
            var movieschedule = _context.MovieSchedules.ToList();
            var rnd = new Random();

            foreach (var itemCustomer in customer)
            {
                booking.Add(new Booking() {Customer = itemCustomer, MovieSchedule = movieschedule[rnd.Next(0,movieschedule.Count)]});
            }
            


            return booking;
        }


        #region bookingseats

        

        private int CheckSeat(int seatId, int movieId)
        {
            var bookingseats = _context.BookingSeats.ToList();
            var bookings = _context.Bookings.ToList();
            var seats = _context.Seats.ToList();

            

            foreach (var bookingseat in bookingseats)
            {
                if (bookingseat.SeatId == seatId)
                {
                    foreach (var booking in bookings)
                    {
                        if (booking.MovieSchedule.Movie.Id == movieId)
                        {
                            
                        }
                    }
                }
            }


            return 22;
        }

        private List<BookingSeat> populateBookingSeats()
        {
            var bookingseats = _context.BookingSeats.ToList();
            var bookings = _context.Bookings.ToList();
            var seats = _context.Seats.ToList();
            var rnd = new Random();

            foreach (var booking in bookings) //Lav et sæde til hver film booking
            {
                int seatid = rnd.Next(0, seats.Count);
                //Tjek at seat ikke er tildelt samme movie
               
                //Lav select i stedet
                //foreach (var bookingseat in bookingseats)
                //{
                //    if (bookingseat.Booking.MovieSchedule.Movie.Id != booking.MovieSchedule.Movie.Id && bookingseat.Seat.Id != seatid)
                //    {
                //        bookingseats.Add(new BookingSeat() { Booking = booking, Seat = seats[seatid]});
                //    }
                
                //}


            }

            return null;

        }

        private List<BookingSeat> populateBookingSeats2()
        {
            var bookingseats = _context.BookingSeats.ToList();
            var bookings = _context.Bookings.ToList();
            var seats = _context.Seats.ToList();
            var rnd = new Random();

            foreach (var booking in bookings)
            {
                int roll = rnd.Next(0, seats.Count);
                if (!bookingseats.Select(x => x.BookingId).Contains(roll))
                {
                    bookingseats.Add(new BookingSeat() { Booking = booking, Seat = seats[roll] });
                }

            }

            return bookingseats;
        }

        #endregion


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

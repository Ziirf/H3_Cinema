using Cinema.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;

namespace Cinema.Api.ExtentionMethods
{
    static class IncludeExtentions
    {
        /// <summary>
        /// Takes a Movie data set and includes all the relevant models to it.
        /// </summary>
        /// <param name="movies">Data set of movie</param>
        /// <returns>IIncludableQueryable that includes all relevant dbSets</returns>
        public static IIncludableQueryable<Movie, Crew> IncludeAll(this DbSet<Movie> movies)
        {
            return movies
                .Include(x => x.AgeRating)?
                .Include(x => x.MovieGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MovieCrews).ThenInclude(x => x.Role)
                .Include(x => x.MovieCrews).ThenInclude(x => x.Crew);
        }

        /// <summary>
        /// Takes a Screenings data set and includes all the relevant models to it.
        /// </summary>
        /// <param name="screenings">Data set of screenings</param>
        /// <returns>IIncludableQueryable that includes all relevant dbSets</returns>
        public static IIncludableQueryable<Screening, Theater> IncludeAll(this DbSet<Screening> screenings)
        {
            return screenings
                    .Include(x => x.Movie).ThenInclude(x => x.AgeRating)
                    .Include(x => x.Seats).ThenInclude(x => x.SeatLocation)
                    .Include(x => x.Theater);
        }

        /// <summary>
        /// Takes a Customer data set and includes all the relevant models to it.
        /// </summary>
        /// <param name="customers">Data set of customer</param>
        /// <returns>IIncludableQueryable that includes all relevant dbSets</returns>
        public static IIncludableQueryable<Customer, ICollection<Booking>> IncludeAll(this DbSet<Customer> customers)
        {
            return customers
                .Include(x => x.Postcode)
                .Include(x => x.Bookings);
        }

        /// <summary>
        /// Takes a Booking data set and includes all the relevant models to it.
        /// </summary>
        /// <param name="movies">Data set of booking</param>
        /// <returns>IIncludableQueryable that includes all relevant dbSets</returns>
        public static IIncludableQueryable<Booking, Movie> IncludeAll(this DbSet<Booking> booking)
        {
            return booking
                .Include(x => x.Customer)
                .Include(x => x.Seat).ThenInclude(x => x.Screening)
                .ThenInclude(x => x.Theater)
                .Include(x => x.Seat)
                .ThenInclude(x => x.SeatLocation)
                .Include(x => x.Seat.Screening.Movie);
        }
    }
}

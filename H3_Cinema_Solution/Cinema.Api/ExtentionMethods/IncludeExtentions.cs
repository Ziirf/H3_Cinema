using Cinema.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;

namespace Cinema.Api.ExtentionMethods
{
    static class IncludeExtentions
    {
        public static IIncludableQueryable<Movie, Crew> IncludeAll(this DbSet<Movie> movies)
        {
            return movies
                .Include(x => x.AgeRating)?
                .Include(x => x.MovieGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MovieCrews).ThenInclude(x => x.Role)
                .Include(x => x.MovieCrews).ThenInclude(x => x.Crew);
        }

        public static IIncludableQueryable<Screening, Theater> IncludeAll(this DbSet<Screening> screenings)
        {
            return screenings
                    .Include(x => x.Movie)
                    .Include(x => x.Seats).ThenInclude(x => x.SeatLocation)
                    .Include(x => x.Theater);
        }

        public static IIncludableQueryable<Customer, ICollection<Booking>> IncludeAll(this DbSet<Customer> customers)
        {
            return customers
                .Include(x => x.Postcode)
                .Include(x => x.Bookings);
        }
    }
}

using Cinema.Converters;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cinema.Converter
{
    public class BookingConverter : IConverter<Booking, BookingDTO>
    {
        private readonly CinemaContext _context;


        public BookingConverter(CinemaContext context)
        {
            _context = context;
        }


        public BookingDTO Convert(Booking booking)
        {
            //Get Bookings from database with relation tables.
            //var bookings = _context.Bookings
            //    .Include(x => x.Customer)
            //    .Include(x => x.Seat).ThenInclude(x => x.Screening)
            //    .ThenInclude(x => x.Theater)
            //    .Include(x => x.Seat)
            //    .ThenInclude(x => x.SeatLocation)
            //    .Include(x=>x.Seat.Screening.Movie)
            //    .FirstOrDefault(x => x.Id == booking.Id);

            // Convert to BookingDTO
            return new BookingDTO()
            {
                BookingId = booking.Id,
                CustomerId = booking.Customer.Id,
                MovieId = booking.Seat.Screening.Movie.Id,
                SeatId = booking.Seat.Id,
                ScreeningId = booking.Seat.ScreeningId,
                SeatNumber = booking.Seat.SeatLocation.SeatNumber,
                RowNumber = booking.Seat.SeatLocation.Row,
                MovieName = booking.Seat.Screening.Movie.Title,
                TheaterName = booking.Seat.Screening.Theater.TheaterName,
                ImgUrl = booking.Seat.Screening.Movie.ImgUrl,
                ScreeningTime = booking.Seat.Screening.Time
            };
        }

        public Booking Convert(BookingDTO bookingDTO)
        {
            // Convert To Booking, get Seat and Customer from CinemaContext by ID
            return new Booking() //TODO
            {
                Id = bookingDTO.BookingId,
                Customer = _context.Customers.FirstOrDefault(x=> x.Id == bookingDTO.CustomerId),
                Seat = _context.Seats.FirstOrDefault(x=> x.Id == bookingDTO.SeatId)
                
            };
        }
    }
}

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
            var bookings = _context.Bookings
                .Include(x => x.Customer)
                .Include(x => x.Seat).ThenInclude(x => x.Screening)
                .ThenInclude(x => x.Theater)
                .Include(x => x.Seat)
                .ThenInclude(x => x.SeatLocation)
                .Include(x=>x.Seat.Screening.Movie)
                .FirstOrDefault(x => x.Id == booking.Id);



            // Convert to BookingDTO
            return new BookingDTO()
            {
                BookingId = booking.Id,
                CustomerId = bookings.Customer.Id,
                MovieId = bookings.Seat.Screening.Movie.Id,
                SeatId = bookings.Seat.Id,
                ScreeningId = bookings.Seat.ScreeningId,
                SeatNumber = bookings.Seat.SeatLocation.SeatNumber,
                RowNumber = bookings.Seat.SeatLocation.Row,
                MovieName = bookings.Seat.Screening.Movie.Title,
                TheaterName = bookings.Seat.Screening.Theater.TheaterName,
                ImgUrl = bookings.Seat.Screening.Movie.ImgUrl,
                ScreeningTime = bookings.Seat.Screening.Time
            };

        }

        public Booking Convert(BookingDTO bookingDTO)
        {
            // Convert To Booking, get Seat and Customer from CinemaContext by ID
            return new Booking() //TODO
            {
                //Id = bookingDTO.BookingId,
                //SeatId = bookingDTO.SeatId,
                //Seat =  Seat() { Id = bookingDTO.SeatId, ScreeningId = bookingDTO.ScreeningId},
                //Customer = _context.Customers.FirstOrDefault(x => x.Id == bookingDTO.CustomerId)

            };
        }


    }
}

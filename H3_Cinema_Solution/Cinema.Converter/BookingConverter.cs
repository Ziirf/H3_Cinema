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
            // Dependency Injection
            _context = context;
        }

        /// <summary>
        /// Converts Booking to BookingDTO
        /// </summary>
        /// <param name="booking">The booking you want converted.</param>
        /// <returns>The Converted booking as DTO</returns>
        public BookingDTO Convert(Booking booking)
        {
            // Convert to BookingDTO
            return new BookingDTO()
            {
                Id = booking.Id,
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

        /// <summary>
        /// Convert BookingDTO to Booking Model
        /// </summary>
        /// <param name="bookingDTO">The booking dto you want converted.</param>
        /// <returns>The Converted DTO as a model</returns>
        public Booking Convert(BookingDTO bookingDTO)
        {
            return new Booking()
            {
                Id = bookingDTO.Id,
                SeatId = bookingDTO.SeatId,
                CustomerId = bookingDTO.CustomerId,
                // grabs the models from the database by Id.
                Customer = _context.Customers.FirstOrDefault(x=> x.Id == bookingDTO.CustomerId),
                Seat = _context.Seats.FirstOrDefault(x=> x.Id == bookingDTO.SeatId)
            };
        }
    }
}

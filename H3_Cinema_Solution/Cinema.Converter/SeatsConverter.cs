using Cinema.Converters;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Cinema.Converter
{
    class SeatsConverter : IConverter<Seat, SeatDTO>
    {
        private readonly CinemaContext _context;
        public SeatsConverter(CinemaContext context)
        {
            _context = context;
        }

        public SeatDTO Convert(Seat seat)
        {
            // Convert Seats to DTO

            var booking = _context.Bookings
                .Include(x => x.Customer).ThenInclude(x => x.Postcode)
                .FirstOrDefault(x => x.SeatId == seat.Id);

            var seatDTO = new SeatDTO
            {
                Id = seat.Id,
                RowNumber = seat.SeatLocation.Row,
                SeatNumber = seat.SeatLocation.SeatNumber,
                IsBooked = booking != null
            };
            if (booking != null)
            {
                var customerConverter = new CustomerConverter(_context);

                var customerDTO = customerConverter.Convert(booking.Customer);
                seatDTO.Customer = customerDTO;
            }

            return seatDTO;
        }

        public Seat Convert(SeatDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}

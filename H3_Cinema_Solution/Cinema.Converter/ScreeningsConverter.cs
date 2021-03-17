using System.Collections.Generic;
using System.Linq;
using Cinema.Converters;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;

namespace Cinema.Converter
{
    public class ScreeningsConverter : IConverter<Screening, ScreeningDTO>
    {
        private readonly CinemaContext _context;
        public ScreeningsConverter(CinemaContext context)
        {
            _context = context;
        }

        public ScreeningDTO Convert(Screening screening)
        {
            var seatConverter = new SeatsConverter(_context);

            return new ScreeningDTO
            {
                Id = screening.Id,
                Time = screening.Time,
                Movie = screening.Movie.Title,
                Theater = screening.Theater.TheaterName,
                Seats = screening.Seats.Select(seat => seatConverter.Convert(seat)).OrderBy(x => x.RowNumber).ThenBy(x => x.SeatNumber).ToList()
            };
        }

        public Screening Convert(ScreeningDTO dto)
        {
            throw new System.NotImplementedException();
        }
    }
}
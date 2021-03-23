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
            // Convert Screening to DTO
            var seatConverter = new SeatsConverter(_context);

            return new ScreeningDTO
            {
                Id = screening.Id,
                Time = screening.Time,
                Movie = screening.Movie.Title,
                AgeRating = screening.Movie.AgeRating.RatingName,
                Theater = screening.Theater.TheaterName,
                Seats = screening.Seats.Select(seat => seatConverter.Convert(seat)).OrderBy(x => x.RowNumber).ThenBy(x => x.SeatNumber).ToList()
            };
        }

        public Screening Convert(ScreeningDTO screeningDTO)
        {
            // Convert DTO to Screening

            // Get Movie and Theater, that corresponds to the Screening
            Movie movie = _context.Movies.FirstOrDefault(x => x.Title == screeningDTO.Movie);
            Theater theater = _context.Theaters.FirstOrDefault(x => x.TheaterName == screeningDTO.Theater);
            Screening screening = new Screening
            {
                Id = screeningDTO.Id,
                Movie = movie,
                Theater = theater,
                Time = screeningDTO.Time
            };

            // Get all seats that the Theater contains and add them to Seats for the Screening
            screening.Seats = new List<Seat>();
            var seatLocations = _context.SeatLocations
                .Where(x => x.Row <= screening.Theater.Row && x.SeatNumber <= screening.Theater.SeatNumber)
                .OrderBy(x => x.Row).ThenBy(x => x.SeatNumber).ToList();

            foreach (var seatLocation in seatLocations)
            {
                screening.Seats.Add(new Seat() { SeatLocation = seatLocation });
            }

            return screening;
        }
    }
}
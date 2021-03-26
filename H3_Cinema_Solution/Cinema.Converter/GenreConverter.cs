using Cinema.Converters;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using System.Linq;

namespace Cinema.Converter
{
    public class GenreConverter : IConverter<Genre, GenreDTO>
    {
        private readonly CinemaContext _context;
        public GenreConverter(CinemaContext context)
        {
            _context = context;
        }

        public GenreDTO Convert(Genre genre)
        {
            // Convert to DTO from genre
            return new GenreDTO
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        public Genre Convert(GenreDTO genreDTO)
        {
            //Convert to Genre and add MovieGenres
            return new Genre
            {
                Id = genreDTO.Id,
                Name = genreDTO.Name,
                MovieGenres = _context.MovieGenres.Where(x => x.GenreId == genreDTO.Id).ToList()
            };
        }
    }
}

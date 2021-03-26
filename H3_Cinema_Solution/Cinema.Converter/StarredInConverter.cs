using Cinema.Converters;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using System.Linq;

namespace Cinema.Converter
{
    class StarredInConverter : IConverter<Movie, StarredInDTO>
    {
        private readonly CinemaContext _context;

        public StarredInConverter(CinemaContext context)
        {
            _context = context;
        }

        public StarredInDTO Convert(Movie movie)
        {
            // Connvert Movie to StarredInDTO
            return new StarredInDTO()
            {
                MovieId = movie.Id,
                MovieName = movie.Title,
                imgUrl = movie.ImgUrl
            };
        }

        public Movie Convert(StarredInDTO starredInDto)
        {
            // Return Movie corresponding to StarredIn
            return _context.Movies.FirstOrDefault(x => x.Id == starredInDto.MovieId);
        }
    }
}

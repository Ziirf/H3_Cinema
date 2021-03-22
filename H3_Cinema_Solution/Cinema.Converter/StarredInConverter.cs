using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Cinema.Converters;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;

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
            return new StarredInDTO()
            {
                MovieId = movie.Id,
                MovieName = movie.Title,
                imgUrl = movie.ImgUrl
            };
        }

        public Movie Convert(StarredInDTO starredInDto)
        {
            return _context.Movies.FirstOrDefault(x => x.Id == starredInDto.MovieId);
        }
    }
}

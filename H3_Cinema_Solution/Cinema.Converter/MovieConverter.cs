using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Cinema.Converters
{
    public class MovieConverter : IConverter<Movie, MovieDTO>
    {
        private CinemaContext _context;
        public MovieConverter()
        {
            _context = new CinemaContext();
        }

        public MovieDTO Convert(Movie movie)
        {
            // Converts a movie Into a into a movieDTO
            return new MovieDTO()
            {
                Id = movie.Id,
                Title = movie.Title,
                Runtime = movie.Runtime,
                Rating = movie.Rating,
                AgeRating = movie.AgeRating?.RatingName,
                ImgUrl = movie.ImgUrl,
                ReleaseDate = movie.ReleaseDate,
                Genre = movie.MovieGenres.Select(x => x.Genre.Name).ToList(),
                Directors = movie.MovieCrews.Where(item => item.Role.Title == "Director").Select(item => item.Crew).ToList(),
                ScreenWriters = movie.MovieCrews.Where(item => item.Role.Title == "Screen Writer").Select(item => item.Crew).ToList(),
                ScriptWriters = movie.MovieCrews.Where(item => item.Role.Title == "Script Writer").Select(item => item.Crew).ToList(),
                Actors = movie.MovieCrews.Where(item => item.Role.Title == "Actor").Select(item => item.Crew).ToList()
            };
        }

        public Movie Convert(MovieDTO movieDTO)
        {

            var crews = new List<MovieCrew>();
            if (movieDTO.Directors != null)
            {
                crews.AddRange(movieDTO.Directors.Select(x => new MovieCrew
                {
                    MovieId = movieDTO.Id,
                    RoleId = 1,
                    CrewId = x.Id
                }));
            }

            if (movieDTO.ScreenWriters != null)
            {
                crews.AddRange(movieDTO.ScreenWriters.Select(x => new MovieCrew
                {
                    MovieId = movieDTO.Id,
                    RoleId = 2,
                    CrewId = x.Id
                }));
            }

            if (movieDTO.ScriptWriters != null)
            {
                crews.AddRange(movieDTO.ScriptWriters.Select(x => new MovieCrew
                {
                    MovieId = movieDTO.Id,
                    RoleId = 3,
                    CrewId = x.Id
                }));
            }

            if (movieDTO.Actors != null)
            {
                crews.AddRange(movieDTO.Actors.Select(x => new MovieCrew
                {
                    MovieId = movieDTO.Id,
                    RoleId = 4,
                    CrewId = x.Id
                }));
            }

            var movie = new Movie
            {
                Id = movieDTO.Id,
                Title = movieDTO.Title,
                Runtime = movieDTO.Runtime,
                Rating = movieDTO.Rating,
                ImgUrl = movieDTO.ImgUrl,
                ReleaseDate = movieDTO.ReleaseDate,
                MovieGenres = movieDTO.Genre.Select(x => new MovieGenre() { Genre = new Genre() { Name = x } }).ToList(),
                MovieCrews = crews.ToList()
            };

            AgeRating ageRating = _context.AgeRatings.FirstOrDefault(x => x.RatingName == movieDTO.AgeRating);
            if (ageRating != null)
            {
                movie.AgeRatingId = ageRating.Id;
            }

            return movie;
        }
    }
}

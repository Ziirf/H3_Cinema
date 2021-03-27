using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Cinema.Converters
{
    public class MovieConverter : IConverter<Movie, MovieDTO>
    {
        private readonly CinemaContext _context;
        public MovieConverter(CinemaContext context)
        {
            _context = context;
        }

        public MovieDTO Convert(Movie movie)
        {
            // Converts a movie Into a into a movieDTO and add references
            return new MovieDTO()
            {
                Id = movie.Id,
                Title = movie.Title,
                Runtime = movie.Runtime,
                Rating = movie.Rating,
                AgeRating = movie.AgeRating?.RatingName,
                ImgUrl = movie.ImgUrl,
                ReleaseDate = movie.ReleaseDate,
                Genre = movie.MovieGenres?.Select(x => x.Genre.Name).ToList(),
                Description = movie.Description,
                Directors = movie.MovieCrews.Where(item => item.Role.Title == "Director").Select(item => item.Crew).ToList(),
                ScreenWriters = movie.MovieCrews.Where(item => item.Role.Title == "Screen Writer").Select(item => item.Crew).ToList(),
                ScriptWriters = movie.MovieCrews.Where(item => item.Role.Title == "Script Writer").Select(item => item.Crew).ToList(),
                Actors = movie.MovieCrews.Where(item => item.Role.Title == "Actor").Select(item => item.Crew).ToList()
            };
        }

        public Movie Convert(MovieDTO movieDTO)
        {
            // Convert to DTO
            var crews = new List<MovieCrew>();
            //Add roles to the Movie, if they exist in the DTO
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
                Description = movieDTO.Description,
                MovieGenres = new List<MovieGenre>(),
                MovieCrews = crews.ToList()
            };

            AgeRating ageRating = _context.AgeRatings.FirstOrDefault(x => x.RatingName == movieDTO.AgeRating);
            // Add Agerating to the Movie, if exist in DTO
            if (ageRating != null)
            {
                movie.AgeRatingId = ageRating.Id;
            }

            // Add Genre to the Movie, if exist in DTO
            if (movieDTO.Genre != null)
            {
                foreach (var genre in movieDTO.Genre)
                {
                    var genreId = _context.Genres.FirstOrDefault(x => x.Name == genre).Id;

                    movie.MovieGenres.Add(new MovieGenre
                    {
                        MovieId = movieDTO.Id,
                        GenreId = genreId
                    });
                }
            }

            return movie;
        }
    }
}

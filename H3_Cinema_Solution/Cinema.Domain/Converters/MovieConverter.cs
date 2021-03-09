﻿using System.Collections.Generic;
using System.Linq;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;

namespace Cinema.Domain.Converters
{
    public class MovieConverter : IConverter<Movie, MovieDTO>
    {
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
                    RoleId = 1,
                    CrewId = x.Id
                }));
            }

            if (movieDTO.ScreenWriters != null)
            {
                crews.AddRange(movieDTO.ScreenWriters.Select(x => new MovieCrew
                {
                    RoleId = 2,
                    CrewId = x.Id
                }));
            }

            if (movieDTO.ScriptWriters != null)
            {
                crews.AddRange(movieDTO.ScriptWriters.Select(x => new MovieCrew
                {
                    RoleId = 3,
                    CrewId = x.Id
                }));
            }

            if (movieDTO.Actors != null)
            {
                crews.AddRange(movieDTO.Actors.Select(x => new MovieCrew
                {
                    RoleId = 4,
                    CrewId = x.Id
                }));
            }


            var movie = new Movie()
            {
                Id = movieDTO.Id,
                Title = movieDTO.Title,
                Runtime = movieDTO.Runtime,
                Rating = movieDTO.Rating,
                AgeRating = new AgeRating(){ RatingName = movieDTO.AgeRating },
                ImgUrl = movieDTO.ImgUrl,
                ReleaseDate = movieDTO.ReleaseDate,
                MovieGenres = movieDTO.Genre.Select(x => new MovieGenre() { Genre = new Genre() { Name = x } }).ToList(),
                MovieCrews = crews.ToList()
            };

            return movie;
        }

        public Movie Transfer(Movie movie, MovieDTO movieDTO)
        {
            var crews = new List<MovieCrew>();
            if (movieDTO.Directors != null)
            {
                crews.AddRange(movieDTO.Directors.Select(x => new MovieCrew
                {
                    MovieId = movie.Id,
                    RoleId = 1,
                    CrewId = x.Id
                }));
            }

            if (movieDTO.ScreenWriters != null)
            {
                crews.AddRange(movieDTO.ScreenWriters.Select(x => new MovieCrew
                {
                    MovieId = movie.Id,
                    RoleId = 2,
                    CrewId = x.Id
                }));
            }

            if (movieDTO.ScriptWriters != null)
            {
                crews.AddRange(movieDTO.ScriptWriters.Select(x => new MovieCrew
                {
                    MovieId = movie.Id,
                    RoleId = 3,
                    CrewId = x.Id
                }));
            }

            if (movieDTO.Actors != null)
            {
                crews.AddRange(movieDTO.Actors.Select(x => new MovieCrew
                {
                    MovieId = movie.Id,
                    RoleId = 4,
                    CrewId = x.Id
                }));
            }


            movie.Id = movieDTO.Id;
            movie.Title = movieDTO.Title;
            movie.Runtime = movieDTO.Runtime;
            movie.Rating = movieDTO.Rating;
            movie.AgeRating = new AgeRating() {RatingName = movieDTO.AgeRating};
            movie.ImgUrl = movieDTO.ImgUrl;
            movie.ReleaseDate = movieDTO.ReleaseDate;
            movie.MovieGenres = movieDTO.Genre.Select(x => new MovieGenre() {Genre = new Genre() {Name = x}}).ToList();
            movie.MovieCrews = crews.ToList();

            return movie;
        }
    }
}

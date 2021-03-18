using Cinema.Converters;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Converter
{
    public class CrewConverter : IConverter<Crew, CrewDTO>
    {
        private readonly CinemaContext _context;
        
        public CrewConverter(CinemaContext context)
        {
            _context = context;
        }


        public CrewDTO Convert(Crew crew)
        {
            List<MovieCrew> moviecrew = new List<MovieCrew>();
 
            moviecrew = _context.MovieCrew.Where(x => x.Crew.Id == crew.Id)
                .Include(x => x.Role)
                .Include(x => x.Movie)
                .ToList();
            var movies = new List<Movie>();
            var roles = new List<string>();


            foreach (var role in moviecrew)
            {
                if (!roles.Contains(role.Role.Title))
                {
                    roles.Add(role.Role.Title);
                }
            }

            foreach (var movie in moviecrew)
            {
                if (!movies.Contains(movie.Movie))
                {
                    movies.Add(movie.Movie);
                }

            }

            var conver = new MovieConverter(_context);

            var crewDTO = new CrewDTO
            {

                Id = crew.Id,
                FirstName = crew.FirstName,
                LastName = crew.LastName,
                Birthday = crew.Birthday,
                ImgUrl = crew.ImgUrl,
                Description = crew.Description,
                Roles = roles,
                StarredIn = movies.Select(x=>conver.Convert(x)).ToList() //Maybe make a StarredInDTO

            };

            return crewDTO;
        }

        public Crew Convert(CrewDTO crewDTO)
        {
            var crew = new Crew
            {
                Id = crewDTO.Id,
                FirstName = crewDTO.FirstName,
                LastName = crewDTO.LastName,
                Birthday = crewDTO.Birthday,
                ImgUrl = crewDTO.ImgUrl,
                Description = crewDTO.Description

            };

            return crew;
        }


    }
}

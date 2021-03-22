using Cinema.Converters;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Cinema.Converter
{
    public class CrewConverter : IConverter<Crew, CrewDTO>
    {
        private readonly CinemaContext _context;
        private readonly StarredInConverter _converterStarred;
        
        public CrewConverter(CinemaContext context)
        {
            _context = context;
            _converterStarred = new StarredInConverter(context);
        }


        public CrewDTO Convert(Crew crew)
        {
            List<MovieCrew> moviecrew = new List<MovieCrew>();

            moviecrew = _context.MovieCrew.Where(x => x.Crew.Id == crew.Id)
                .Include(x => x.Role)
                .Include(x => x.Movie)
                .ToList();
            var roles = new List<string>();

            var starredin = new List<StarredInDTO>();

            foreach (var role in moviecrew)
            {
                if (!roles.Contains(role.Role.Title))
                {
                    roles.Add(role.Role.Title);
                }
            }

            foreach (var movie in moviecrew)
            {
                if (!starredin.Contains(_converterStarred.Convert(movie.Movie)))
                {
                    starredin.Add(_converterStarred.Convert(movie.Movie));
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
                StarredIn = starredin

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema.Converter;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Query;

namespace Cinema.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrewsController : ControllerBase
    {
        private readonly CinemaContext _context;
        private readonly CrewConverter _converter;

        public CrewsController(CinemaContext context)
        {
            _context = context;
            _converter = new CrewConverter(context);
        }

        // GET: api/Crews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CrewDTO>>> GetCrews()
        {
            // Get crews from Database and convert to crewDTO
            var crews = await _context.Crews.ToListAsync();

            return crews.Select(crew => _converter.Convert(crew)).ToList();
        }


        // GET: api/Crews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CrewDTO>> GetCrew(int id)
        {
            // Get specific Crew and convert to crewDTO
            var crew = await _context.Crews.FindAsync(id);

            if (crew == null)
            {
                return NotFound();
            }

            return _converter.Convert(crew);
        }

        /// <summary>
        /// Updates Crew via CrewDTO.
        /// DOES NOT UPDATE STARREDIN OR ROLES, MUST BE DONES VIA MOVIES
        /// </summary>
        /// <param name="id"></param>
        /// <param name="crewDTO"></param>
        /// <returns></returns>
        // PUT: api/Crews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCrew(int id, CrewDTO crewDTO)
        {
            //Update specific Crew, does not update starredINDTO. 
            if (id != crewDTO.Id)
            {
                return BadRequest();
            }

            Crew crew = _converter.Convert(crewDTO);


            _context.Entry(crew).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CrewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Post a new Crew.
        /// STARREDIN AND ROLES WILL NOT BE POSTED,
        /// MUST BE DONE VIA MOVIES
        /// </summary>
        /// <param name="crewDTO"></param>
        /// <returns></returns>
        // POST: api/Crews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CrewDTO>> PostCrew(CrewDTO crewDTO)
        {
            Crew crew = _converter.Convert(crewDTO);

            _context.Crews.Add(crew);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCrew", new { id = crew.Id }, crew);
        }


        // DELETE: api/Crews/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCrew(int id)
        {
            // Deletes specific crew and its relations to moviescrew
            List<MovieCrew> movieCrews = _context.MovieCrew.Where(x => x.CrewId == id).ToList();

            var crew = await _context.Crews.FindAsync(id);
            if (crew == null)
            {
                return NotFound();
            }
            
            //Remove relation to moviecrews and removes the crew
            _context.RemoveRange(movieCrews);
            _context.Crews.Remove(crew);


            await _context.SaveChangesAsync();

            return NoContent();
        }

        

        private bool CrewExists(int id)
        {
            return _context.Crews.Any(e => e.Id == id);
        }
    }
}

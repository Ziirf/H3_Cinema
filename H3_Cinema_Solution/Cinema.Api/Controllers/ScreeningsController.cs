using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Converter;
using Cinema.Converters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace Cinema.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreeningsController : ControllerBase
    {
        private readonly CinemaContext _context;
        private readonly ScreeningsConverter _screeningsConverter;

        public ScreeningsController(CinemaContext context)
        {
            _context = context;
            _screeningsConverter = new ScreeningsConverter(context);
        }

        // GET: api/Screenings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScreeningDTO>>> GetScreenings()
        {
            var screenings = await GetScreeningsFromContext().ToListAsync();

            return screenings.Select(x => _screeningsConverter.Convert(x)).ToList();
        }

        // GET: api/Screenings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScreeningDTO>> GetScreening(int id)
        {
            var screening = await GetScreeningsFromContext().FirstOrDefaultAsync(x => x.Id == id);

            if (screening == null)
            {
                return NotFound();
            }

            return _screeningsConverter.Convert(screening);
        }

        // PUT: api/Screenings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutScreening(int id, Screening screening)
        {
            if (id != screening.Id)
            {
                return BadRequest();
            }

            _context.Entry(screening).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScreeningExists(id))
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

        // POST: api/Screenings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Screening>> PostScreening(Screening screening)
        {
            _context.Screenings.Add(screening);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetScreening", new { id = screening.Id }, screening);
        }

        // DELETE: api/Screenings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScreening(int id)
        {
            var screening = await _context.Screenings.FindAsync(id);
            if (screening == null)
            {
                return NotFound();
            }

            _context.Screenings.Remove(screening);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private IIncludableQueryable<Screening, Theater> GetScreeningsFromContext()
        {
            // Get the entire model plus its relevant relations.
            return _context.Screenings
                .Include(x => x.Movie)
                .Include(x => x.Seats).ThenInclude(x => x.SeatLocation)
                .Include(x => x.Theater);
        }

        private bool ScreeningExists(int id)
        {
            return _context.Screenings.Any(e => e.Id == id);
        }
    }
}

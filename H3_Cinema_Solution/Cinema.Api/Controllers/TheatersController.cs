using Cinema.Data;
using Cinema.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TheatersController : ControllerBase
    {
        private readonly CinemaContext _context;

        public TheatersController(CinemaContext context)
        {
            _context = context;
        }

        // GET: api/Theaters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Theater>>> GetTheaters()
        {
            // Get Theaters from database
            return await _context.Theaters.ToListAsync();
        }

        // GET: api/Theaters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Theater>> GetTheater(int id)
        {
            // Get specific Theater by id from database
            var theater = await _context.Theaters.FindAsync(id);

            if (theater == null)
            {
                return NotFound();
            }

            return theater;
        }

        // PUT: api/Theaters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTheater(int id, Theater theater)
        {
            // Update Theater
            if (id != theater.Id)
            {
                return BadRequest();
            }

            _context.Entry(theater).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TheaterExists(id))
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

        // POST: api/Theaters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Theater>> PostTheater(Theater theater)
        {
            // Post theater
            _context.Theaters.Add(theater);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTheater", new { id = theater.Id }, theater);
        }


        // DELETE: api/Theaters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheater(int id)
        {
            // Delete theater and remove Screenings that contain the theater
            List<Screening> screenings = await _context.Screenings.Include(x => x.Theater)
                .Where(x => x.Theater.Id == id).ToListAsync();


            var theater = await _context.Theaters.FindAsync(id);
            if (theater == null)
            {
                return NotFound();
            }

            _context.RemoveRange(screenings);
            _context.Theaters.Remove(theater);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TheaterExists(int id)
        {
            return _context.Theaters.Any(e => e.Id == id);
        }
    }
}

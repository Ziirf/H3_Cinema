using Cinema.Api.ExtentionMethods;
using Cinema.Converter;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

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
            // Get Screeningsd and include relations from database. Convert to DTO
            var screenings = await _context.Screenings.IncludeAll().ToListAsync();

            var result = screenings.Select(x => _screeningsConverter.Convert(x)).ToList();
            
            // If not admin remove Customer
            if (!User.IsInRole("Admin"))
            {
                foreach (var item in result.SelectMany(screening => screening.Seats))
                {
                    item.Customer = null;
                }
            }

            return result;
        }

        // GET: api/Screenings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScreeningDTO>> GetScreening(int id)
        {
            // Get specific Screening and include relations from database. Convert to DTO
            var screening = await _context.Screenings.IncludeAll().FirstOrDefaultAsync(x => x.Id == id);

            var screeningDTO = _screeningsConverter.Convert(screening);
            
            // If not admin remove Customer
            if (!User.IsInRole("Admin"))
            {
                foreach (var item in screeningDTO.Seats)
                {
                    item.Customer = null;
                }
            }


            if (screening == null)
            {
                return NotFound();
            }

            return screeningDTO;
        }

        // GET: api/Screenings/Movie/5

        [HttpGet("Movie/{id}")]
        public async Task<ActionResult<IEnumerable<ScreeningDTO>>> GetScreeningByMovieId(int id)
        {
            // Get Screenings with specific movie and include relations from database. convert to DTO
            var screenings = await _context.Screenings.IncludeAll().Where(x => x.Movie.Id == id).ToListAsync();
            var result = screenings.Select(x => _screeningsConverter.Convert(x)).ToList();

            // If not admin remove Customer
            if (!User.IsInRole("Admin"))
            {
                foreach (var item in result.SelectMany(screening => screening.Seats))
                {
                    item.Customer = null;
                }
            }

            if (screenings == null)
            {
                return NotFound();
            }

            return result;
        }

        [Authorize(Roles = "Admin")]
        // PUT: api/Screenings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScreening(int id, ScreeningDTO screeningDTO)
        {
            // Update Screening 
            var amountBooked = _context.Bookings.Where(x => x.Seat.ScreeningId == id).Count();

            if (id != screeningDTO.Id)
            {
                return BadRequest();
            }

            // Check that that the screening does not have booked seats
            if (amountBooked > 0)
            {
                return Problem("Can't edit a movie that got bookings");
            }

            Screening screening = _screeningsConverter.Convert(screeningDTO);
            //_context.RemoveRange(_context.Seats.Where(x => x.ScreeningId == id));

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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Screening>> PostScreening(ScreeningDTO screeningDTO)
        {
            //Post a Screening
            Screening screening = _screeningsConverter.Convert(screeningDTO);

            _context.Screenings.Add(screening);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetScreening", new { id = screening.Id }, _screeningsConverter.Convert(screening));
        }

        // DELETE: api/Screenings/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScreening(int id)
        {
            // Delete a specific Screening
            var screening = await _context.Screenings.FindAsync(id);
            if (screening == null)
            {
                return NotFound();
            }

            _context.Screenings.Remove(screening);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ScreeningExists(int id)
        {
            return _context.Screenings.Any(e => e.Id == id);
        }
    }
}

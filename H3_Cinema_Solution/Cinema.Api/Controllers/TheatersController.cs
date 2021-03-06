using Cinema.Data;
using Cinema.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheatersController : ControllerBase
    {
        private readonly CinemaContext _context;

        public TheatersController(CinemaContext context)
        {
            _context = context;
        }


        //TODO: Test delete og put

        // GET: api/Theaters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Theater>>> GetTheaters()
        {
            var theaters = _context.Theaters
                    .Include(x => x.Seats).ThenInclude(x => x.SeatLocation).ToListAsync();

            return await theaters;
        }

        // GET: api/Theaters
        [HttpGet("name")]
        public async Task<ActionResult<IEnumerable<Theater>>> GetTheatersName()
        {
            var theaters = await _context.Theaters
                    .Include(x => x.Seats).ThenInclude(x => x.SeatLocation)
                    .Select(data => new
                    {
                        data.TheaterName
                    }).ToListAsync();

            return Ok(theaters);
        }

        // GET: api/Theaters
        [HttpGet("seats")]
        public async Task<ActionResult<IEnumerable<Theater>>> GetTheatersSeats()
        {
            var theaters = await _context.Theaters
                    .Include(x => x.Seats).ThenInclude(x => x.SeatLocation)
                    .Select(data => new
                    {
                        data.TheaterName,
                        data.Seats,
                        allseat = data.Seats.Select(seatObj => new
                        {
                            seatObj.SeatLocation.Row,
                            seatObj.SeatLocation.Seat
                        })

                    }).ToListAsync();

            return Ok(theaters);
        }

        // GET: api/Theaters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Theater>> GetTheater(int id)
        {
            var theater = _context.Theaters.Include(x => x.Seats).ThenInclude(x => x.SeatLocation).FirstOrDefaultAsync(x => x.Id == id);

            if (theater == null)
            {
                return NotFound();
            }

            return await theater;
        }

        // PUT: api/Theaters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTheater(int id, Theater theater)
        {
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
            _context.Theaters.Add(theater);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTheater", new { id = theater.Id }, theater);
        }

        // DELETE: api/Theaters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheater(int id)
        {
            var theater = await _context.Theaters.Include(x => x.Seats).ThenInclude(x => x.SeatLocation).FirstOrDefaultAsync(x => x.Id == id);
            if (theater == null)
            {
                return NotFound();
            }

            //_context.RemoveRange(theater.Seats, theater); Remove more than one
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

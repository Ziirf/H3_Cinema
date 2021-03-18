﻿using Cinema.Converter;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq;
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

        // GET: api/Screenings/Movie/5
        [HttpGet("Movie/{id}")]
        public async Task<ActionResult<IEnumerable<ScreeningDTO>>> GetScreeningByMovieId(int id)
        {
            var screenings = await GetScreeningsFromContext().Where(x => x.Movie.Id == id).ToListAsync();

            if (screenings == null)
            {
                return NotFound();
            }

            return screenings.Select(x => _screeningsConverter.Convert(x)).ToList();
        }

        // PUT: api/Screenings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScreening(int id, ScreeningDTO screeningDTO)
        {
            //var asd = screeningDTO.Seats.Where(x => x.IsBooked == true).Count();
            var amountBooked = _context.Bookings.Where(x => x.Seat.ScreeningId == id).Count();

            if (id != screeningDTO.Id)
            {
                return BadRequest();
            }

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
        [HttpPost]
        public async Task<ActionResult<Screening>> PostScreening(ScreeningDTO screeningDTO)
        {
            Screening screening = _screeningsConverter.Convert(screeningDTO);

            _context.Screenings.Add(screening);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetScreening", new { id = screening.Id }, _screeningsConverter.Convert(screening));
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
        }

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

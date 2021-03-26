using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Converter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cinema.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly CinemaContext _context;
        private readonly GenreConverter _converter;

        public GenresController(CinemaContext context)
        {
            _context = context;
            _converter = new GenreConverter(_context);
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetGenres()
        {
            // Get all genres and convert them to GenreDTO
            var genre = await _context.Genres.ToListAsync();

            return genre.Select(x => _converter.Convert(x)).ToList();
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<string>>> GetGenresList()
        {
            // Gets all genres and returns a list of strings
            return await _context.Genres.Select(x => x.Name).ToListAsync();
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDTO>> GetGenre(int id)
        {
            // Get a specific genre and convert to GenreDTO
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return _converter.Convert(genre);
        }

        // PUT: api/Genres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, Genre genre)
        {
            // Update a genre
            if (id != genre.Id)
            {
                return BadRequest();
            }

            // Check if genre already exist.
            if (_context.Genres.FirstOrDefault(x => x.Name == genre.Name) != null)
            {
                return Problem(title: "This genre name already exists");
            }

            _context.Entry(genre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
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

        // POST: api/Genres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GenreDTO>> PostGenre(GenreDTO genreDTO)
        {
            // Post a Genre

            // Check that the genre does not already exist
            if (_context.Genres.FirstOrDefault(x => x.Name == genreDTO.Name) != null)
            {
                return Problem(title: "This genre name already exists");
            }

            // Convert from GenreDTO to Genre and save
            var genre = _converter.Convert(genreDTO);
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGenre", new { id = genre.Id }, _converter.Convert(genre));
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            // Delete specific Genre by ID
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.Id == id);
        }
    }
}

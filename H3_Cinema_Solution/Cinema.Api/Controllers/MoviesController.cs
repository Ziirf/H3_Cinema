using Cinema.Converters;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly CinemaContext _context;
        private readonly MovieConverter _converter;

        public MoviesController(CinemaContext context)
        {
            _context = context;
            _converter = new MovieConverter();
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
        {
            // Gets the movies out of the database.
            var movies = GetMoviesFromContext();

            return await movies.Select(movie => _converter.Convert(movie)).ToListAsync();
        }

        // GET: api/Movies/Random
        // GET: api/Movies/Random/10
        [HttpGet("Random")]
        [HttpGet("Random/{amount}")]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesRandom(int amount = 1)
        {
            // Gets the movies out of the database and puts it into a list.
            var movies = GetMoviesFromContext();

            // Picks out random movies and converts them into MovieDTO objects and returns them in a list.
            var random = new Random();

            return await movies.OrderBy(x => random.Next()).Take(amount).Select(movie => _converter.Convert(movie)).ToListAsync();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovie(int id)
        {
            // Gets the first movie with the Id into a single.
            Movie movie = await GetMoviesFromContext().FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            // Converts the movie into a movieDTO and returns it.
            return _converter.Convert(movie);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieDTO movieDTO)
        {
            // Checks if the Id's are matching.
            if (id != movieDTO.Id)
            {
                return BadRequest();
            }

            // Converts into a movie model.
            Movie movie = _converter.Convert(movieDTO);

            // Removes the many to many relations.
            _context.RemoveRange(_context.MovieCrew.Where(x => x.MovieId == movieDTO.Id));
            _context.RemoveRange(_context.MovieGenres.Where(x => x.MovieId == movieDTO.Id));

            // Adding new many to many relations.
            _context.MovieCrew.AddRange(movie.MovieCrews);
            _context.MovieGenres.AddRange(movie.MovieGenres);

            // Updating the entry
            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!MovieExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(MovieDTO movieDTO)
        {
            // Converts into a movie model.
            Movie movie = _converter.Convert(movieDTO);

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            // Get back the movie including its relation to return the movieDTO.
            movie = await GetMoviesFromContext().FirstOrDefaultAsync(x => x.Id == movie.Id);
            movieDTO = _converter.Convert(movie);

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movieDTO);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            // Get the relevant movie.
            Movie movie = GetMoviesFromContext().FirstOrDefault(x => x.Id == id);

            // Get the relevant screenings that are relevant to the movie.
            List<Screening> screenings = await _context.Screenings.Include(x => x.Movie)
                .Where(x => x.Movie.Id == id).ToListAsync();

            if (movie == null)
            {
                return NotFound();
            }

            // Deletes the movie and the screenings.
            _context.RemoveRange(screenings);
            _context.Remove(movie);

            // Save the changes.
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private IIncludableQueryable<Movie, Crew> GetMoviesFromContext()
        {
            // Get the entire model plus its relevant relations.
            return _context.Movies
                .Include(x => x.AgeRating)?
                .Include(x => x.MovieGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MovieCrews).ThenInclude(x => x.Role)
                .Include(x => x.MovieCrews).ThenInclude(x => x.Crew);
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
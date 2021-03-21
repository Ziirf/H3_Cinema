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
using Cinema.Api.ExtentionMethods;

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
            _converter = new MovieConverter(context);
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
        {
            // Gets the movies out of the database.
            return await _context.Movies.IncludeAll().Select(movie => _converter.Convert(movie)).ToListAsync();
        }

        // Get: api/Movies/Range/1-20
        [HttpGet("Range/{start}-{end}")]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesRange(int start, int end)
        {
            // Checks if the range is valid.
            if (start <= 0 || end <= start)
            {
                return BadRequest();
            }

            // Gets the movies out of the database.
            return await _context.Movies.IncludeAll().Select(movie => _converter.Convert(movie)).Skip(start - 1).Take(end - start + 1).ToListAsync();
        }

        // Get: api/Movies/Page/1
        [HttpGet("Page/{page}")]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesPage(int page)
        {
            // Checks if the page is valid.
            if (page <= 0 )
            {
                return BadRequest();
            }

            // Gets the movies out of the database.
            return await _context.Movies.IncludeAll().Select(movie => _converter.Convert(movie)).Skip(20 * (page -1)).Take(20).ToListAsync();
        }

        [HttpGet("Genre/{genre}")]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesByGenre(string genre)
        {
            // Gets all the movies that have a genre equal to the input.
            var movies = await _context.Movies.IncludeAll().Where(x => x.MovieGenres.Any(y => y.Genre.Name.ToLower() == genre.ToLower())).ToListAsync();

            // Converts them into DTO.
            return movies.Select(movie => _converter.Convert(movie)).ToList();
        }

        // GET: api/Movies/Random
        // GET: api/Movies/Random/10
        [HttpGet("Random")]
        [HttpGet("Random/{amount}")]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesRandom(int amount = 20)
        {
            // Gets the movies out of the database and puts it into a list.
            var movies = await _context.Movies.IncludeAll().ToListAsync();

            // Picks out random movies and converts them into MovieDTO objects and returns them in a list.
            var rnd = new Random();

            return movies.ToList().OrderBy(x => rnd.Next()).Take(amount)
                .Select(movie => _converter.Convert(movie)).ToList();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovie(int id)
        {
            // Gets the first movie with the Id into a single.
            Movie movie = await _context.Movies.IncludeAll().FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            // Converts the movie into a movieDTO and returns it.
            return _converter.Convert(movie);
        }

        // GET: api/Airing
        [HttpGet("Airing")]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovieCurrentlyAiring()
        {
            // Gets all the id's of movies with a screening.
            var listMovieId = await _context.Screenings.Include(x => x.Movie).Select(x => x.Movie.Id).Distinct().ToListAsync();
            // Gets all movies with above ids.
            var movies = await _context.Movies.IncludeAll().Where(x => listMovieId.Contains(x.Id)).ToListAsync();

            return movies.Select(movie => _converter.Convert(movie)).ToList();
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

            // marks the entry as modified
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
        public async Task<ActionResult<MovieDTO>> PostMovie(MovieDTO movieDTO)
        {
            // Converts into a movie model.
            Movie movie = _converter.Convert(movieDTO);

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            // Get back the movie including its relation to return the movieDTO.
            movie = await _context.Movies.IncludeAll().FirstOrDefaultAsync(x => x.Id == movie.Id);
            movieDTO = _converter.Convert(movie);

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movieDTO);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            // Get the relevant movie.
            Movie movie = _context.Movies.IncludeAll().FirstOrDefault(x => x.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            // Get the relevant screenings that are relevant to the movie.
            List<Screening> screenings = await _context.Screenings.Include(x => x.Movie)
                .Where(x => x.Movie.Id == id).ToListAsync();

            // Deletes the movie and the screenings.
            _context.RemoveRange(screenings);
            _context.Remove(movie);

            // Save the changes.
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
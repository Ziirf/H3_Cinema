using System;
using Cinema.Data;
using Cinema.Domain.Converters;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace Cinema.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly CinemaContext _context;

        public MoviesController(CinemaContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
        {
            // Gets the movies out of the database and puts it into a list.
            var movies = await GetMoviesFromContext().ToListAsync();

            // Converts the movies into MovieDTO objects and returns them in a list.
            var converter = new MovieConverter();

            return movies.Select(movie => converter.Convert(movie)).ToList();
        }

        [HttpGet("Random")]
        [HttpGet("Random/{amount}")]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesRandom(int amount = 1)
        {
            // Gets the movies out of the database and puts it into a list.
            var movies = await GetMoviesFromContext().ToListAsync();

            // Picks out random movies and converts them into MovieDTO objects and returns them in a list.
            var converter = new MovieConverter();
            var random = new Random();
            return movies.OrderBy(x => random.Next()).Take(amount).Select(movie => converter.Convert(movie)).ToList();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovie(int id)
        {
            // Gets the first movie with the Id into a single.
            var movie = await GetMoviesFromContext().FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            // Converts the movie into a movieDTO and returns it.
            var converter = new MovieConverter();

            return converter.Convert(movie);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieDTO movieDTO)
        {
            if (id != movieDTO.Id)
            {
                return BadRequest();
            }

            // Gets the first movie with the Id into a single.
            var movie = await GetMoviesFromContext().FirstOrDefaultAsync(x => x.Id == id);

            // Removes the many to many relations.
            _context.RemoveRange(movie.MovieCrews.ToList());
            _context.RemoveRange(movie.MovieGenres.ToList());

            // Transfers the data from the DTO to the DOCO without making a new instance.
            var converter = new MovieConverter();
            movie = converter.Transfer(movie, movieDTO);

            if (movie == null)
            {
                return NotFound();
            }

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
            var converter = new MovieConverter();

            // Convert the DTO into DOCO in order to store it in the database.
            var movie = converter.Convert(movieDTO);
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            // Get back the movie including its relation to return the movieDTO object.
            movie = await GetMoviesFromContext().FirstOrDefaultAsync(x => x.Id == movie.Id);

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movieDTO);
        }

        // TODO DELETE
        // DELETE: api/Movies/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteMovie(int id)
        //{
        //    var movie = _context.Movies
        //        .Include(x => x.MovieGenres)
        //        .Include(x => x.MovieCrews)
        //        .FirstOrDefault(x => x.Id == id);

        //    //var bookings = _context.Bookings.Include(x => x.Screening).ThenInclude(x => x.Movie).Where(x => x.Screening.Movie.Id == id).ToList();

        //    var movieScheduels = _context.Screenings.Include(x => x.Movie)
        //        .Where(x => x.Movie.Id == id).ToList();


        //    //var movieDTO = await _context.Movies.FindAsync(id);
        //    if (movie == null)
        //    {
        //        return NotFound();
        //    }

        //    //_context.RemoveRange(bookings);
        //    _context.RemoveRange(movieScheduels);
        //    _context.RemoveRange(movie.MovieGenres);
        //    _context.RemoveRange(movie.MovieCrews);
        //    _context.Remove(movie);

        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private IIncludableQueryable<Movie, Crew> GetMoviesFromContext()
        {
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
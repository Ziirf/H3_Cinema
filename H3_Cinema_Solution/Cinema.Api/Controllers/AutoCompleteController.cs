using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Domain.Models;
using Cinema.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Api.Controllers
{
    // Authorize (attribute), checking if you are admin
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AutoCompleteController : ControllerBase
    {
        private readonly CinemaContext _context;

        public AutoCompleteController(CinemaContext context)
        {
            // Dependency injection
            _context = context;
        }

        // GET: api/AutoComplete/Movies/  -- In case there are no search term it will call same controller but with no input.
        // GET: api/AutoComplete/Movies/(searchTerm)
        [HttpGet("Movies/")]
        [HttpGet("Movies/{search}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovieAutoComplete(string search = "")
        {
            // Get the first 50 movies that contains the searchterm in the title, output with only id and value to decrease latency.
            var movies = await _context.Movies.Select(x => new { id = x.Id, value = x.Title })
                .Where(x => x.value.Contains(search)).Take(50)
                .ToListAsync();

            return Ok(movies);
        }

        // GET: api/AutoComplete/Genres/  -- In case there are no search term it will call same controller but with no input.
        // GET: api/AutoComplete/Genres/(searchTerm)
        [HttpGet("Genres/")]
        [HttpGet("Genres/{search}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetGenreAutoComplete(string search = "")
        {
            // Get the first 50 genres that contains the searchterm in the name, output with only id and value to decrease latency.
            var genres = await _context.Genres.Select(x => new { id = x.Id, value = x.Name })
                .Where(x => x.value.Contains(search)).Take(50)
                .ToListAsync();

            return Ok(genres);
        }

        // GET: api/AutoComplete/Users/  -- In case there are no search term it will call same controller but with no input.
        // GET: api/AutoComplete/Users/(searchTerm)
        [HttpGet("Users")]
        [HttpGet("Users/{search}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersAutoComplete(string search = "")
        {
            // Get the first 50 users that contains the searchterm in the username, output with only id and value to decrease latency.
            var users = await _context.Users.Select(x => new { id = x.Id, value = x.Username })
                .Where(x => x.value.Contains(search)).Take(50)
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/AutoComplete/Customers/  -- In case there are no search term it will call same controller but with no input.
        // GET: api/AutoComplete/Customers/(searchTerm)
        [HttpGet("Customers")]
        [HttpGet("Customers/{search}")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersAutoComplete(string search = "")
        {
            // Get the first 50 customers that contains the searchterm in the Firstname and Lastname, output with only id and value to decrease latency.
            var customers = await _context.Customers.Select(x => new { id = x.Id, value = x.FirstName + " " + x.LastName })
                .Where(x => x.value.Contains(search)).Take(50)
                .ToListAsync();

            return Ok(customers);
        }

        // GET: api/AutoComplete/Theaters/  -- In case there are no search term it will call same controller but with no input.
        // GET: api/AutoComplete/Theaters/(searchTerm)
        [HttpGet("Theaters")]
        [HttpGet("Theaters/{search}")]
        public async Task<ActionResult<IEnumerable<Theater>>> GetTheatersAutoComplete(string search = "")
        {
            // Get the first 50 theaters that contains the searchterm in the Theatername, output with only id and value to decrease latency.
            var theaters = await _context.Theaters.Select(x => new { id = x.Id, value = x.TheaterName })
                .Where(x => x.value.Contains(search)).Take(50)
                .ToListAsync();

            return Ok(theaters);
        }
    }
}

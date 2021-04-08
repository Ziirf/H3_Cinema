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
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AutoCompleteController : ControllerBase
    {
        private readonly CinemaContext _context;

        public AutoCompleteController(CinemaContext context)
        {
            _context = context;
        }

        [HttpGet("Movies/")]
        [HttpGet("Movies/{search}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovieAutoComplete(string search = "")
        {
            var movies = await _context.Movies.Select(x => new { id = x.Id, value = x.Title })
                .Where(x => x.value.Contains(search)).Take(50)
                .ToListAsync();

            return Ok(movies);
        }

        [HttpGet("Genres/")]
        [HttpGet("Genres/{search}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetGenreAutoComplete(string search = "")
        {
            var genres = await _context.Genres.Select(x => new { id = x.Id, value = x.Name })
                .Where(x => x.value.Contains(search)).Take(50)
                .ToListAsync();

            return Ok(genres);
        }

        [HttpGet("Users")]
        [HttpGet("Users/{search}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersAutoComplete(string search = "")
        {
            var users = await _context.Users.Select(x => new { id = x.Id, value = x.Username })
                .Where(x => x.value.Contains(search)).Take(50)
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("Customers")]
        [HttpGet("Customers/{search}")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersAutoComplete(string search = "")
        {
            var customers = await _context.Customers.Select(x => new { id = x.Id, value = $"{x.FirstName} {x.LastName}" })
                .Where(x => x.value.Contains(search)).Take(50)
                .ToListAsync();

            return Ok(customers);
        }

        [HttpGet("Theater")]
        [HttpGet("Theater/{search}")]
        public async Task<ActionResult<IEnumerable<Theater>>> GetTheatersAutoComplete(string search = "")
        {
            var theaters = await _context.Theaters.Select(x => new { id = x.Id, value = $"{x.TheaterName}" })
                .Where(x => x.value.Contains(search)).Take(50)
                .ToListAsync();

            return Ok(theaters);
        }
    }
}

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

        [HttpGet("Movies")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovieAutoComplete()
        {
            var movies = await _context.Movies.Select(x => new {id = x.Id, value = x.Title}).ToListAsync();
            
            return Ok(movies);
        }

        //[HttpGet("Screenings")]
        //public async Task<ActionResult<IEnumerable<Screening>>> GetScreeningAutoComplete()
        //{
        //    var screenings = await _context.Screenings.Select(x => new { id = x.Id, value = x. }).ToListAsync();

        //    return Ok(movies);
        //}

        [HttpGet("Users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersAutoComplete()
        {
            // Get all movie and convert them to Movies
            var users = await _context.Users.Select(x => new { id = x.Id, value = x.Username }).ToListAsync();

            return Ok(users);
        }

        [HttpGet("Customers")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersAutoComplete()
        {
            var customers = await _context.Customers.Select(x => new { id = x.Id, value = $"{x.FirstName} {x.LastName}" }).ToListAsync();

            return Ok(customers);
        }

        [HttpGet("Theater")]
        public async Task<ActionResult<IEnumerable<Theater>>> GetTheatersAutoComplete()
        {
            var theaters = await _context.Theaters.Select(x => new { id = x.Id, value = $"{x.TheaterName}" }).ToListAsync();

            return Ok(theaters);
        }
    }
}

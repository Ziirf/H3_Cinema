using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema.Data;
using Cinema.Domain.Models;

namespace Cinema.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgeRatingsController : ControllerBase
    {
        private readonly CinemaContext _context;

        public AgeRatingsController(CinemaContext context)
        {
            _context = context;
        }

        // GET: api/AgeRatings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgeRating>>> GetAgeRatings()
        {
            return await _context.AgeRatings.ToListAsync();
        }

        // GET: api/AgeRatings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AgeRating>> GetAgeRating(int id)
        {
            var ageRating = await _context.AgeRatings.FindAsync(id);

            if (ageRating == null)
            {
                return NotFound();
            }

            return ageRating;
        }

    }
}

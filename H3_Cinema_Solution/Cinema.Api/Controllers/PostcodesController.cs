using Cinema.Data;
using Cinema.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinema.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostcodesController : ControllerBase
    {
        private readonly CinemaContext _context;

        public PostcodesController(CinemaContext context)
        {
            _context = context;
        }

        // GET: api/Postcodes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Postcode>>> GetPostcodes()
        {
            // Get all postcodes from database
            return await _context.Postcodes.ToListAsync();
        }

        // GET: api/Postcodes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Postcode>> GetPostcode(int id)
        {
            // Get specific postcode from database by ID
            var postcode = await _context.Postcodes.FindAsync(id);

            if (postcode == null)
            {
                return NotFound();
            }

            return postcode;
        }

    }
}

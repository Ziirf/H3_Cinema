using Cinema.Converter;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Api.ExtentionMethods;

namespace Cinema.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly CinemaContext _context;
        private readonly BookingConverter _converter;

        public BookingsController(CinemaContext context)
        {
            _context = context;
            _converter = new BookingConverter(_context);
        }

        // GET: api/Bookings
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBookings()
        {
            // Get all bookings and convert to BookingDTO
            var bookings = await _context.Bookings.IncludeAll().ToListAsync();

            return bookings.Select(x => _converter.Convert(x)).ToList();
        }

        // Get: api/Bookings/Customer/1
        [HttpGet("Customer/{id}")]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBookingsByCustomerId(int id)
        {
            // Checks if the customerId isn't the same as the queried Id and if the user isn't Admin.
            if (!User.IsInRole("Admin") && User.FindFirst("CustomerId").Value != id.ToString())
            {
                return BadRequest();
            }

            // Get all bookings and convert to BookingDTO
            var bookings = await _context.Bookings.IncludeAll().ToListAsync();

            bookings = bookings.Where(x => x.Customer.Id == id).ToList();

            return bookings.Select(x => _converter.Convert(x)).ToList();
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BookingDTO>> GetBooking(int id)
        {
            var booking = await _context.Bookings.IncludeAll().FirstOrDefaultAsync(x => x.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            return _converter.Convert(booking);
        }

        [Authorize(Roles = "Admin")]
        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, BookingDTO bookingDTO)
        {
            // Update a Booking
            if (id != bookingDTO.BookingId)
            {
                return BadRequest();
            }

            // Convert into Booking from BookingDTO
            Booking booking = _converter.Convert(bookingDTO);

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(BookingDTO bookingDTO)
        {
            // Checks if the customerId isn't the same as the queried Id and if the user isn't Admin.
            if (!User.IsInRole("Admin") && User.FindFirst("CustomerId").Value != bookingDTO.CustomerId.ToString())
            {
                return BadRequest();
            }

            // Converts BookingDTO into Booking
            Booking booking = _converter.Convert(bookingDTO);

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            // Checks if the customerId isn't the same as the queried Id and if the user isn't Admin.
            if (!User.IsInRole("Admin") && User.FindFirst("CustomerId").Value != id.ToString())
            {
                return BadRequest();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}

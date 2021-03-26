using Cinema.Converter;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Api.ExtentionMethods;
using Microsoft.AspNetCore.Authorization;

namespace Cinema.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CinemaContext _context;
        private readonly CustomerConverter _converter;

        public CustomersController(CinemaContext context)
        {
            _context = context;
            _converter = new CustomerConverter(context);
        }

        // GET: api/Customers
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            // Gets the customers out of the database.
            var customers = await _context.Customers.IncludeAll().ToListAsync();

            return customers.Select(x => _converter.Convert(x)).OrderBy(x => x.Id).ToList();
        }


        // GET: api/Customers/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(int id)
        {
            // Checks if the customerId isn't the same as the queried Id and if the user isn't Admin.
            if (!User.IsInRole("Admin") && User.FindFirst("CustomerId").Value != id.ToString())
            {
                return BadRequest();
            }

            // Gets the first customer with the Id into a single.
            Customer customer = await _context.Customers.IncludeAll().FirstOrDefaultAsync(x => x.Id == id);
            
            if (customer == null)
            {
                return NotFound();
            }

            // Converts the customer into a customerDTO and returns it.
            return _converter.Convert(customer);
        }

        // PUT: api/Customers/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerDTO customerDTO)
        {
            // Checks if the customerId isn't the same as the queried Id and if the user isn't Admin.
            if (!User.IsInRole("Admin") && User.FindFirst("CustomerId").Value != id.ToString())
            {
                return BadRequest();
            }
            // Checks if the Id's are matching.
            if (id != customerDTO.Id)
            {
                return BadRequest();
            }

            // Converts into a customer model.
            Customer customer = _converter.Convert(customerDTO);

            // Updating the entry.
            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> PostCustomer(CustomerDTO customerDTO)
        {
            // Converts into a customer model.
            Customer customer = _converter.Convert(customerDTO);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Get back the customer including its relation to return the customerDTO.
            customer = await _context.Customers.IncludeAll().FirstOrDefaultAsync(x => x.Id == customer.Id);

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, _converter.Convert(customer));
        }

        [Authorize]
        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            // Checks if the customerId isn't the same as the queried Id and if the user isn't Admin.
            if (!User.IsInRole("Admin") && User.FindFirst("CustomerId").Value != id.ToString())
            {
                return BadRequest();
            }
            // Get the relevant customer.
            Customer customer = await _context.Customers.IncludeAll().FirstOrDefaultAsync(x => x.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            // Deletes the customer.
            _context.Remove(customer);

            // Save the change.
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
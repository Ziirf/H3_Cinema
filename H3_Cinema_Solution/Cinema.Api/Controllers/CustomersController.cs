using Cinema.Data;
using Cinema.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Converter;
using Cinema.Domain.DTOs;
using Microsoft.EntityFrameworkCore.Query;

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
            _converter = new CustomerConverter();
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            var customers = await GetCustomersFromContext().ToListAsync();

            return customers.Select(x => _converter.Convert(x)).OrderBy(x => x.Id).ToList();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(int id)
        {
            var customer = await GetCustomersFromContext().FirstOrDefaultAsync(x => x.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return _converter.Convert(customer);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

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
            Customer customer = _converter.Convert(customerDTO);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            customer = await GetCustomersFromContext().FirstOrDefaultAsync(x => x.Id == customer.Id);
            
            //return CreatedAtAction("GetCustomer", new { id = customer.Id }, _converter.Convert(customer));
            return  _converter.Convert(customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.Include(x => x.Postcode).FirstOrDefaultAsync(x => x.Id == id);
            //var customerDTO = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private IIncludableQueryable<Customer, Postcode> GetCustomersFromContext()
        {
            return _context.Customers.Include(x => x.Postcode);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
using Cinema.Data;
using Cinema.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace Cinema.Test.Database
{
    public class CustomerDbTest
    {
        private CinemaContext _context;
        public CustomerDbTest()
        {
            var builder = new DbContextOptionsBuilder<CinemaContext>();
            builder.UseInMemoryDatabase("CinemaDBCustomerTesting");
            _context = new CinemaContext(builder.Options);
        }

        [Fact]
        public async Task CanAddACustomerToDatabase()
        {
            var customer = new Customer()
            {
                FirstName = "Nicolai",
                LastName = "Friis"
            };
            _context.Add(customer);
            await _context.SaveChangesAsync();
            Assert.NotEqual(0, customer.Id);
        }

        [Fact]
        public async Task CanReadACustomerFromDatabase()
        {
            var customer = new Customer()
            {
                FirstName = "Nicolai",
                LastName = "Friis"
            };
            _context.Add(customer);
            await _context.SaveChangesAsync();
            Assert.Equal(_context.Customers.Find(customer.Id), customer);
        }

        [Fact]
        public async Task CanEditACustomerInDatabase()
        {
            var customer = new Customer()
            {
                FirstName = "Nicolai",
                LastName = "Friis"
            };
            _context.Add(customer);
            await _context.SaveChangesAsync();
            Assert.Equal(customer, _context.Customers.Find(customer.Id));

            customer.FirstName = "Something";
            customer.LastName = "Else";
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            Assert.Equal(customer, _context.Customers.Find(customer.Id));
        }

        [Fact]
        public async Task CanDeleteACustomerFromDatabase()
        {
            var customer = new Customer()
            {
                FirstName = "Nicolai",
                LastName = "Friis"
            };
            _context.Add(customer);
            await _context.SaveChangesAsync();
            Assert.Equal(customer, _context.Customers.Find(customer.Id));

            _context.Remove(customer);
            await _context.SaveChangesAsync();
            Assert.Null(_context.Customers.Find(customer.Id));
        }
    }
}

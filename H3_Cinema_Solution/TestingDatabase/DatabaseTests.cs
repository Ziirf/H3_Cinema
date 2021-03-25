using NUnit.Framework;
using Cinema.Data;
using Cinema.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace TestingDatabase
{
    public class DatabaseTests
    {
        private CinemaContext _context;

        [SetUp]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder<CinemaContext>();
            builder.UseInMemoryDatabase("CinemaMemoryDB"); 
            _context = new CinemaContext(builder.Options);
        }

        [Test]
        public void CanAddACustomerToDatabase()
        {
            var customer = new Customer()
            {
                FirstName = "Nicolai",
                LastName = "Friis"
            };
            _context.Add(customer); 
            _context.SaveChanges();
            Assert.AreNotEqual(0, customer.Id);
        }

        [Test]
        public void CanReadACustomerFromDatabase()
        {
            var customer = new Customer()
            {
                FirstName = "Nicolai",
                LastName = "Friis"
            };
            _context.Add(customer);
            _context.SaveChanges();
            Assert.AreEqual(_context.Customers.Find(customer.Id), customer);
        }

        [Test]
        public void CanEditACustomerInDatabase()
        {
            var customer = new Customer()
            {
                FirstName = "Nicolai",
                LastName = "Friis"
            };
            _context.Add(customer);
            _context.SaveChanges();
            Assert.AreEqual(customer, _context.Customers.Find(customer.Id));

            customer.FirstName = "Something";
            customer.LastName = "Else";
            _context.Entry(customer).State = EntityState.Modified;
            _context.SaveChanges();

            Assert.AreEqual(customer, _context.Customers.Find(customer.Id));
        }

        [Test]
        public void CanDeleteACustomerFromDatabase()
        {
            var customer = new Customer()
            {
                FirstName = "Nicolai",
                LastName = "Friis"
            };
            _context.Add(customer);
            _context.SaveChanges();
            Assert.AreEqual(customer, _context.Customers.Find(customer.Id));

            _context.Remove(customer);
            _context.SaveChanges();
            Assert.AreEqual(null, _context.Customers.Find(customer.Id));
        }
    }
}
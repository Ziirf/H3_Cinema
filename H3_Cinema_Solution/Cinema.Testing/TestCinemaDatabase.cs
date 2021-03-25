using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cinema.Data;
using Cinema.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Testing
{
    [TestClass]
    class TestCinemaDatabase
    {
        [TestMethod]
        public void InsertCustomerIntoDatabase()
        {
            using var context = new CinemaContext();
            context.Database.EnsureCreated();
            context.Database.EnsureDeleted();
            var customer = new Customer();
            context.Add(customer);
            Assert.AreNotEqual(0, customer.Id);
        }
    }
}

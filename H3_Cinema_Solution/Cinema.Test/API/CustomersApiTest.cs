using Cinema.Data;
using Cinema.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Api.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Cinema.Test.API
{
    public class CustomersApiTest
    {
        private CinemaContext _context;
        private CustomersController _controller;
        private List<Customer> _customers;

        public CustomersApiTest()
        {
            var builder = new DbContextOptionsBuilder<CinemaContext>();
            builder.UseInMemoryDatabase("CinemaApiCustomerTesting");
            _context = new CinemaContext(builder.Options);
            _controller = new CustomersController(_context);

            var user = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "1"),
                    new Claim("CustomerId", "1"),
                    new Claim(ClaimTypes.Role, "Admin")
                }
            ));

            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            _customers = new List<Customer>()
            {
                new Customer { FirstName = "Nicolai"},
                new Customer { FirstName = "Lauge" },
                new Customer { FirstName = "Sebastian" },
                new Customer { FirstName = "Flemming" }
            };

            _context.AddRange(_customers);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetCustomerById()
        {
            var result = await _controller.GetCustomer(1);

            Assert.Equal(_customers[0].FirstName, result.Value.FirstName);
        }

        [Fact]
        public async Task GetAllCustomers()
        {
            var result = await _controller.GetCustomers();

            Assert.Equal(4, result.Value.Count());
        }
    }
}

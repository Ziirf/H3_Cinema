using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Converter;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Cinema.Test.Converter
{
    public class CustomerConverterTest
    {
        private CinemaContext _context;
        private Customer _customer;

        public CustomerConverterTest()
        {
            var builder = new DbContextOptionsBuilder<CinemaContext>();
            builder.UseInMemoryDatabase("CinemaConverterCustomerTesting");
            _context = new CinemaContext(builder.Options);

            _customer = new Customer()
            {
                FirstName = "Nicolai",
                LastName = "Friis",
                Address = "Address",
                Email = "Mail@Test.com",
                PhoneNumber = "12345678",
                Postcode = new Postcode { City = "TestTown", Code = 1234 },
                Bookings = new List<Booking>
                {
                    new Booking { Seat = new Seat() },
                    new Booking { Seat = new Seat() }
                }
                
            };

            _context.Customers.Add(_customer);
            _context.SaveChangesAsync();
        }

        [Fact]
        public void ConvertToDto()
        {
            var customerConverter = new CustomerConverter(_context);

            var customer = _context.Customers.Find(1);
            var customerDto = customerConverter.Convert(customer);
            Assert.Equal(customer.Id, customerDto.Id);
            Assert.Equal(customer.FirstName, customerDto.FirstName);
            Assert.Equal(customer.LastName, customerDto.LastName);
            Assert.Equal(customer.Address, customerDto.Address);
            Assert.Equal(customer.Postcode.Code, customerDto.Postcode);
            Assert.Equal(customer.Postcode.City, customerDto.City);
            Assert.Equal(customer.PhoneNumber, customerDto.PhoneNumber);
            Assert.Equal(customer.Email, customerDto.Email);
        }

        [Fact]
        public void ConvertFromDto()
        {
            var customerConverter = new CustomerConverter(_context);

            var customerDto = new CustomerDTO()
            {
                Id = 1,
                FirstName = "Nicolai",
                LastName = "Friis",
                Address = "Address",
                Email = "Mail@Test.com",
                PhoneNumber = "12345678",
                City = "TestTown",
                Postcode = 1234,
            };

            var customer = customerConverter.Convert(customerDto);

            Assert.Equal(customerDto.Id, customer.Id);
            Assert.Equal(customerDto.FirstName, customer.FirstName);
            Assert.Equal(customerDto.LastName, customer.LastName);
            Assert.Equal(customerDto.Address, customer.Address);
            Assert.Equal(customerDto.Postcode, customer.Postcode.Code);
            Assert.Equal(customerDto.City, customer.Postcode.City);
            Assert.Equal(customerDto.PhoneNumber, customer.PhoneNumber);
            Assert.Equal(customerDto.Email, customer.Email);
            Assert.Equal(2, customer.Bookings.Count);
        }
    }
}

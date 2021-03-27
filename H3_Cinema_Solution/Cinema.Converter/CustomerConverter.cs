using System.Collections.Generic;
using Cinema.Converters;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using System.Linq;

namespace Cinema.Converter
{
    public class CustomerConverter : IConverter<Customer, CustomerDTO>
    {
        private readonly CinemaContext _context;
        public CustomerConverter(CinemaContext context)
        {
            _context = context;
        }

        public CustomerDTO Convert(Customer customer)
        {
            // Create CustomerDTO
            var customerDTO = new CustomerDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email
            };

            // Convert postcode to dto, if exist.
            if (customer.Postcode != null)
            {
                customerDTO.Postcode = customer.Postcode.Code;
                customerDTO.City = customer.Postcode.City;
            }

            return customerDTO;
        }

        public Customer Convert(CustomerDTO customerDTO)
        {
            // Convert to Customer from DTO
            var customer = new Customer
            {
                Id = customerDTO.Id,
                FirstName = customerDTO.FirstName,
                LastName = customerDTO.LastName,
                Address = customerDTO.Address,
                PhoneNumber = customerDTO.PhoneNumber,
                Email = customerDTO.Email
            };

            Postcode postcode = _context.Postcodes.FirstOrDefault(x => x.Code == customerDTO.Postcode);
            //Add postcode to customer if data exist.
            if (postcode != null)
            {
                customer.PostcodeId = postcode.Id;
                customer.Postcode = postcode;
            }

            List<Booking> bookings = _context.Bookings.Where(x => x.Customer.Id == customer.Id).ToList();
            if (bookings != null)
            {
                customer.Bookings = bookings;
            }

            return customer;
        }
    }
}

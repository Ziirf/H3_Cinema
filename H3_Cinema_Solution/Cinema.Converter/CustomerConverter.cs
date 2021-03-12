using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cinema.Converters;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;

namespace Cinema.Converter
{
    public class CustomerConverter : IConverter<Customer, CustomerDTO>
    {
        private CinemaContext _context;
        public CustomerConverter()
        {
            _context = new CinemaContext();
        }


        public CustomerDTO Convert(Customer customer)
        {
            return new CustomerDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                Postcode = customer.Postcode.Code,
                City = customer.Postcode.City,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email
            };
        }

        public Customer Convert(CustomerDTO customerDTO)
        {
            return new Customer
            {
                Id = customerDTO.Id,
                FirstName = customerDTO.FirstName,
                LastName = customerDTO.FirstName,
                Address = customerDTO.Address,
                PostcodeId = _context.Postcodes.FirstOrDefault(x => x.Code == customerDTO.Postcode).Id,
                PhoneNumber = customerDTO.PhoneNumber,
                Email = customerDTO.Email
            };
        }
    }
}

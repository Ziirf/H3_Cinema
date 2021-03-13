using Cinema.Converters;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using System.Linq;

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
            var customerDTO = new CustomerDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email
            };

            if (customer.Postcode != null)
            {
                customerDTO.Postcode = customer.Postcode.Code;
                customerDTO.City = customer.Postcode.City;
            }

            return customerDTO;
        }

        public Customer Convert(CustomerDTO customerDTO)
        {
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
            if (postcode != null)
            {
                customer.PostcodeId = postcode.Id;
            }

            return customer;
        }

        //public Customer Transfer(Customer customer, CustomerDTO customerDTO)
        //{
        //    customer.Id = customerDTO.Id;
        //    customer.FirstName = customerDTO.FirstName;
        //    customer.LastName = customerDTO.LastName;
        //    customer.Address = customerDTO.Address;
        //    customer.PostcodeId = _context.Postcodes.FirstOrDefault(x => x.Code == customerDTO.Postcode).Id;
        //    customer.PhoneNumber = customerDTO.PhoneNumber;
        //    customer.Email = customerDTO.Email;

        //    return customer;
        //}
    }
}

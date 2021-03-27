using Cinema.Domain.Models;
using Xunit;

namespace Cinema.Test.Models
{
    //[TestFixture]
    public class CustomerModel
    {
        [Theory]
        [InlineData(null, null, null, null, null, 5)]
        [InlineData("Nicolai", null, null, null, null, 4)]
        [InlineData("Nicolai", "Friis", null, null, null, 3)]
        [InlineData("Nicolai", "Friis", "Akelejevej 28", null, null, 2)]
        [InlineData("Nicolai", "Friis", "Akelejevej 28", "test@test.com", null, 1)]
        [InlineData("Nicolai", "Friis", "Akelejevej 28", "test@test.com", "20212223", 0)]
        public void CheckForRequirements(string firstName, string lastName, string address, string email, string phoneNumber, int errors)
        {
            var customer = new Customer()
            {
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                Email = email,
                PhoneNumber = phoneNumber
            };

            var results = Helper.Validate(customer);

            Assert.Equal(errors, results.Count);
        }

        [Theory]
        [InlineData(null, 1)]
        [InlineData("", 1)]
        [InlineData("NotAValidMail", 1)]
        [InlineData("A@Valid.Mail", 0)]
        public void CheckEmailRequirements(string email, int errors)
        {
            var customer = new Customer()
            {
                FirstName = "Nicolai",
                LastName = "lastName",
                Address = "address",
                Email = email,
                PhoneNumber = "20212223"
            };

            var results = Helper.Validate(customer);

            Assert.Equal(errors, results.Count);
        }

        [Theory]
        [InlineData(null, 1)]
        [InlineData("", 1)]
        [InlineData("123", 1)]
        [InlineData("123456789123456789", 1)]
        [InlineData("20212223", 0)]
        public void CheckPhoneRequirements(string phone, int errors)
        {
            var customer = new Customer()
            {
                FirstName = "Nicolai",
                LastName = "lastName",
                Address = "address",
                Email = "valid@mail.com",
                PhoneNumber = phone
            };

            var results = Helper.Validate(customer);

            Assert.Equal(errors, results.Count);
        }
    }
}

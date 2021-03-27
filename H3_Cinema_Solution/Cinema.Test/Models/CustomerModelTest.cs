using Cinema.Domain.Models;
using System.Linq;
using Xunit;

namespace Cinema.Test.Models
{
    //[TestFixture]
    public class CustomerModelTest
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
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("NotAValidMail", false)]
        [InlineData("A@Valid.Mail", true)]
        public void CheckEmailRequirements(string email, bool success)
        {
            var customer = new Customer()
            {
                Email = email
            };

            var result = Helper.HasError(customer, "Email");

            Assert.NotEqual(success, result);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("123", false)]
        [InlineData("123456789123456789", false)]
        [InlineData("20212223", true)]
        public void CheckPhoneRequirements(string phone, bool success)
        {
            var customer = new Customer()
            {
                PhoneNumber = phone
            };

            var result = Helper.HasError(customer, "PhoneNumber");

            Assert.NotEqual(success, result);
        }
    }
}

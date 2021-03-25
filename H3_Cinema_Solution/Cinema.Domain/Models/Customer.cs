using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.Models

{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        public int? PostcodeId { get; set; }
        public Postcode Postcode { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}

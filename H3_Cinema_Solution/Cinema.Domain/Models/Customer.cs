﻿using System.Collections.Generic;

namespace Cinema.Domain.Models

{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int? PostcodeId { get; set; }
        public Postcode Postcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}

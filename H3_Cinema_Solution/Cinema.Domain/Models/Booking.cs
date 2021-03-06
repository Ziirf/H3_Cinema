using System.Collections.Generic;

namespace Cinema.Domain.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public MovieSchedule MovieSchedule { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int SeatId { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Seat Seat { get; set; }
    }
}

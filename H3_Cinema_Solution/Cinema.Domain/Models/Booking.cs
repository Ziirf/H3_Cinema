using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int SeatId { get; set; } 
        [Required]
        public Customer Customer { get; set; }
        [Required]
        public Seat Seat { get; set; }
    }
}

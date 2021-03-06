using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Domain.Models
{
    public class BookingSeat
    {
        public int BookingId { get; set; }
        public int SeatId { get; set; }
        public Booking Booking { get; set; }
        public Seat Seat { get; set; }
    }
}

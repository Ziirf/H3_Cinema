using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Domain.Models
{
    public class SeatLocation
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Seat { get; set; }
    }
}

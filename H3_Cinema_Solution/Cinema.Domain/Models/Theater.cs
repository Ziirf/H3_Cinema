﻿using System.Collections.Generic;

namespace Cinema.Domain.Models
{
    public class Theater
    {
        public int Id { get; set; }
        public string TheaterName { get; set; }

        public int Row { get; set; }

        public int SeatNumber { get; set; }
        //public ICollection<Seat> Seats { get; set; }
    }
}

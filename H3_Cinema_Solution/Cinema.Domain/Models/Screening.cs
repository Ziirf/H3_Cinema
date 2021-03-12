using System;
using System.Collections.Generic;

namespace Cinema.Domain.Models
{
    public class Screening
    {
        public int Id { get; set; }
        public Movie Movie { get; set; }
        public Theater Theater { get; set; }
        public ICollection<Seat> Seats { get; set; }
        public DateTime Time { get; set; }
    }
}

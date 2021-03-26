using System;
using System.Collections.Generic;

namespace Cinema.Domain.DTOs
{
    public class ScreeningDTO
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Movie { get; set; }
        public string Theater { get; set; }
        public string AgeRating { get; set; }
        public ICollection<SeatDTO> Seats { get; set; }
    }
}

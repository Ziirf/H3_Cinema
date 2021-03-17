using System;
using System.Collections.Generic;
using System.Text;
using Cinema.Domain.Models;

namespace Cinema.Domain.DTOs
{
    public class ScreeningDTO
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Movie { get; set; }
        public string Theater { get; set; }
        public ICollection<SeatDTO> Seats { get; set; }
    }
}

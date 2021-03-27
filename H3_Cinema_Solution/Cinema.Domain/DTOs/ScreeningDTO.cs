using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DTOs
{
    public class ScreeningDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A time is required.")]
        public DateTime Time { get; set; }

        [Required(ErrorMessage = "A Movie is required.")]
        public string Movie { get; set; }

        [Required(ErrorMessage = "A Theater is required.")]
        public string Theater { get; set; }

        [Required(ErrorMessage = "AgeRating is required.")]
        public string AgeRating { get; set; }

        public ICollection<SeatDTO> Seats { get; set; }
    }
}

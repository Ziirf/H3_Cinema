using System;

namespace Cinema.Domain.DTOs
{
    public class BookingDTO
    {
        public int BookingId { get; set; }

        public int CustomerId { get; set; }
        public int MovieId { get; set; }
        public int SeatId { get; set; }
        public int ScreeningId { get; set; }

        public int SeatNumber { get; set; }
        public int RowNumber { get; set; }

        public string MovieName { get; set; }
        public string TheaterName { get; set; }

        public string ImgUrl { get; set; }

        public DateTime ScreeningTime { get; set; }
    }
}

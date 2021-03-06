using System;

namespace Cinema.Domain.Models
{
    public class MovieSchedule
    {
        public int Id { get; set; }
        public Movie Movie { get; set; }
        public Theater Theater { get; set; }
        public DateTime Time { get; set; }
    }
}

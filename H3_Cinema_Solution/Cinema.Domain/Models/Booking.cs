namespace Cinema.Domain.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int MovieScheduleId { get; set; }
        public int? SeatId { get; set; } 
        public Customer Customer { get; set; }
        public MovieSchedule MovieSchedule { get; set; }
        public Seat Seat { get; set; }
    }
}

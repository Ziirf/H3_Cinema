namespace Cinema.Domain.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int SeatLocationId { get; set; }
        public int MovieScheduleId { get; set; }
        //public int BookingId { get; set; }
        public SeatLocation SeatLocation { get; set; }
    }
}

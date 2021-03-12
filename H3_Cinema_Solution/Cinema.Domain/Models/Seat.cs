namespace Cinema.Domain.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int SeatLocationId { get; set; }
        public int ScreeningId { get; set; }
        public SeatLocation SeatLocation { get; set; }
        public Screening Screening { get; set; }
    }
}

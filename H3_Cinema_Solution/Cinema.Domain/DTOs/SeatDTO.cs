namespace Cinema.Domain.DTOs
{
    public class SeatDTO
    {
        public int Id { get; set; }
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }
        public bool IsBooked { get; set; }
        public CustomerDTO Customer { get; set; }
    }
}

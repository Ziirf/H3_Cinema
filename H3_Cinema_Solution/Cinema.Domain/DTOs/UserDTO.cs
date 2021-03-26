using Cinema.Domain.Models;

namespace Cinema.Domain.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public UserRights Rights { get; set; }
        public int CustomerId { get; set; }
    }
}

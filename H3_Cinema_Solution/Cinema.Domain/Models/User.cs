namespace Cinema.Domain.Models
{
    public enum UserRights { User = 0, Admin = 1 }
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRights Rights { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}

using System.Collections.Generic;

namespace Cinema.Domain.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<MovieCrew> MovieCrews { get; set; }
    }
}

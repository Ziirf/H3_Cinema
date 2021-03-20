using System;
using System.Collections.Generic;
using System.Text;
using Cinema.Domain.Models;

namespace Cinema.Domain.DTOs
{
    public class CrewDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }

        public List<string> Roles { get; set; }

        public IEnumerable<MovieDTO> StarredIn { get; set; }
    }
}

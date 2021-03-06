using System;
using System.Collections.Generic;

namespace Cinema.Domain.Models
{
    public class Crew
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }
    }
}
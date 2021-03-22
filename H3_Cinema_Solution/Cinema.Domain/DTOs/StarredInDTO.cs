using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Domain.DTOs
{
    public class StarredInDTO
    {
        public int MovieId { get; set; }

        public string MovieName { get; set; }

        public string imgUrl { get; set; }
    }
}

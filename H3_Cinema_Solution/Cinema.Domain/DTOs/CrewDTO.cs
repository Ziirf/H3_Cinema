using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DTOs
{
    public class CrewDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "FirstName is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public string ImgUrl { get; set; }

        public string Description { get; set; }

        public List<string> Roles { get; set; }

        public IEnumerable<StarredInDTO> StarredIn { get; set; }
    }
}

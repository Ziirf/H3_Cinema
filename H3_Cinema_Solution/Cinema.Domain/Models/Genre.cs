using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
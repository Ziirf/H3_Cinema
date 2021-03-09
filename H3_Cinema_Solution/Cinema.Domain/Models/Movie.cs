using System;
using System.Collections.Generic;

namespace Cinema.Domain.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Runtime { get; set; }
        public float Rating { get; set; }
        public AgeRating AgeRating { get; set; }
        public string ImgUrl { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; }
        public ICollection<MovieCrew> MovieCrews { get; set; }
    }
}

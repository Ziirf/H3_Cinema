using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "Title is too long.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Runtime is required.")]
        [Range(1, 600, ErrorMessage = "The runtime is too long")]
        public int Runtime { get; set; }

        [Range(1.0, 10.0, ErrorMessage = "The rating have to be within 0 and 10")]
        public float Rating { get; set; }

        public int? AgeRatingId { get; set; }

        public AgeRating AgeRating { get; set; }

        public string ImgUrl { get; set; }

        public string Description { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }

        public ICollection<MovieCrew> MovieCrews { get; set; }
    }
}

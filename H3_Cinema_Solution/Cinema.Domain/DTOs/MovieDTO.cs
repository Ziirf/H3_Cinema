using Cinema.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DTOs
{
    public class MovieDTO
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
        public string AgeRating { get; set; }

        public string ImgUrl { get; set; }

        public string Description { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public ICollection<string> Genre { get; set; }

        public ICollection<Crew> Directors { get; set; }

        public ICollection<Crew> ScreenWriters { get; set; }

        public ICollection<Crew> ScriptWriters { get; set; }

        public ICollection<Crew> Actors { get; set; }

    }
}

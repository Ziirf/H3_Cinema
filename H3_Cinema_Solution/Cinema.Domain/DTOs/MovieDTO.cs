using Cinema.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Domain.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Runtime { get; set; }
        public float Rating { get; set; }
        public string AgeRating { get; set; }
        public string ImgUrl { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public ICollection<string> Genre { get; set; }
        public ICollection<Crew> Directors { get; set; }
        public ICollection<Crew> ScreenWriters { get; set; }
        public ICollection<Crew> ScriptWriters { get; set; }
        public ICollection<Crew> Actors { get; set; }
    }
}

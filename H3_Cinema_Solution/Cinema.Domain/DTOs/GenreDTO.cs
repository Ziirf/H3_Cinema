using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DTOs
{
    public class GenreDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]

        public string Name { get; set; }
    }
}

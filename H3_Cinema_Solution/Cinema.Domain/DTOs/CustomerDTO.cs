using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "FirstName is required.")]
        [MaxLength(100, ErrorMessage = "FirstName is too long.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        [MaxLength(100, ErrorMessage = "LastName is too long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(100, ErrorMessage = "Address is too long.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Postcode is required.")]
        public int? Postcode { get; set; }
        
        public string City { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [MinLength(8, ErrorMessage = "Phone number is too short.")]
        [MaxLength(8, ErrorMessage = "Phone number is too long.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

    }
}

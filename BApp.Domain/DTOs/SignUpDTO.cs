using System.ComponentModel.DataAnnotations;

namespace BApp.Domain.DTOs
{
    public class SignUpDTO
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

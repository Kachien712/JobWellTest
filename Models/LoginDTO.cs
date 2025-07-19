using System.ComponentModel.DataAnnotations;

namespace JobWell.Models
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(30, ErrorMessage = "Password must be between 6 characters and 30 characters long.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}

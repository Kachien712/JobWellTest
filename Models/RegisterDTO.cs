using System.ComponentModel.DataAnnotations;

namespace JobWell.Models
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name cannot longer than 100 Characters.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(50, ErrorMessage = "Email cannot longer than 100 Characters.")]
        [EmailAddress(ErrorMessage = "Wrong email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(30, ErrorMessage = "Password must be between 6 and 30 Characters.", MinimumLength = 6)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(30, ErrorMessage = "Password must be between 6 and 30 Characters.", MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

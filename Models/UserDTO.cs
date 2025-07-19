using System.ComponentModel.DataAnnotations;

namespace JobWell.Models
{
    public class UserDTO
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        public string FullName { get; set; } 

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } 

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(30, ErrorMessage = "Password must be between 6 and 30 characters long.", MinimumLength = 6)]
        public string Password { get; set; } 
        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [StringLength(30, ErrorMessage = "Password must be  between 6 and 30 characters long.", MinimumLength = 6)]
        public string ConfirmPassword { get; set; }
    }
}

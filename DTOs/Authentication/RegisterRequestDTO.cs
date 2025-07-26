using System.ComponentModel.DataAnnotations;

namespace JobWellTest2.DTOs.Authentication
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "UserName is required.")]
        [MaxLength(30, ErrorMessage = "UserName cannot exceed 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+84\d{9}$", ErrorMessage = "Phone number must start with +84 and 9 digits following.")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Số CCCD phải là 12 chữ số.")]
        public string CitizenIdentityNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

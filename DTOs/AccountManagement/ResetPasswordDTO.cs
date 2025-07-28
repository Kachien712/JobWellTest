using System.ComponentModel.DataAnnotations;

namespace JobWellTest.DTOs.AccountManagement
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Wrong email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Verification code is required.")]
        [RegularExpression(@"^\{6}$", ErrorMessage = "Verification code must be 6 digits.")]
        public string Token { get; set; }

        [Required(ErrorMessage = "New Password is required.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm New Password is required.")]
        [Compare("NewPassword", ErrorMessage = "New Passwords do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}

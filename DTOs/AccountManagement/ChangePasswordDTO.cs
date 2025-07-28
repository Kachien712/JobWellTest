using System.ComponentModel.DataAnnotations;

namespace JobWellTest.DTOs.AccountManagement
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "New Password is required.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm New Password is required.")]
        [Compare("NewPassword", ErrorMessage = "New Passwords do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}

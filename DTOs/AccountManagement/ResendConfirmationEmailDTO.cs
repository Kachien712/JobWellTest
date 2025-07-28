using System.ComponentModel.DataAnnotations;

namespace JobWellTest.DTOs.AccountManagement
{
    public class ResendConfirmationEmailDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Wrong email format.")]
        public string Email { get; set; }
    }
}

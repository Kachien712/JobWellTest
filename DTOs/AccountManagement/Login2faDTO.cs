using System.ComponentModel.DataAnnotations;

namespace JobWellTest.DTOs.Authentication
{
    public class Login2faDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Wrong email format.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d{6}$")]
        public string Code { get; set; }
    }
}

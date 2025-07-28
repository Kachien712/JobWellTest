using System.ComponentModel.DataAnnotations;

namespace JobWellTest.DTOs.Authentication
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}

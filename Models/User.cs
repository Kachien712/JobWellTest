using Microsoft.AspNetCore.Identity;

namespace JobWellTest.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string CitizenIdentityNumber { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; } = DateOnly.MinValue;
    }
}

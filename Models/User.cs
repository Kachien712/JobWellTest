namespace JobWell.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; } 
        public string Email { get; set; } 
        public string PasswordHash { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public int isActive { get; set; } = 0;
        public string? ActivationCode { get; set; }
        public string? PasswordResetCode { get; set; }
        public DateTime? PasswordResetCodeExpiresOn { get; set; }
    }
}

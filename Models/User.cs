namespace JobWell.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; } 
        public string Email { get; set; } 
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public int isActive { get; set; } = 1;
    }
}

namespace JobWellTest.DTOs.UserManagement
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; } 
        public string Email { get; set; } 
        public string PhoneNumber { get; set; }
        public string CitizenIdentityNumber { get; set; }
        public string Token { get; set; }
    }
}

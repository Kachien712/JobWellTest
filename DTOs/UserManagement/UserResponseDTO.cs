namespace JobWellTest2.DTOs.UserManagement
{
    public class UserResponseDTO
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}

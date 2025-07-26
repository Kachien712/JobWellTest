using JobWellTest.DTOs.UserManagement;

namespace JobWellTest.Interfaces
{
    public interface IUserRepository
    {
        public Task<UserDTO> GetUserByIdAsync(string userId);
        public Task<List<UserResponseDTO>> GetAllUsersAsync();
    }
}

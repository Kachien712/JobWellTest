using JobWellTest2.DTOs.UserManagement;

namespace JobWellTest2.Interfaces
{
    public interface IUserRepository
    {
        public Task<UserDTO> GetUserByIdAsync(string userId);
        public Task<List<UserResponseDTO>> GetAllUsersAsync();
    }
}

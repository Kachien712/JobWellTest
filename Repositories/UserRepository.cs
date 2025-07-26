using JobWellTest2.DTOs.UserManagement;
using JobWellTest2.Interfaces;
using JobWellTest2.Mappers;
using JobWellTest2.Models;
using Microsoft.AspNetCore.Identity;

namespace JobWellTest2.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.ToDTO();
        }
        public async Task<List<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            return users.Select(u => u.ToDTO().ToResponseDTO()).ToList();
        }
    }
}

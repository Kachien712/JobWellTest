using JobWellTest2.DTOs.UserManagement;
using JobWellTest2.Models;

namespace JobWellTest2.Mappers
{
    public static class UserMapper
    {
        public static UserDTO ToDTO(this User user)
        {
            return new UserDTO
            {
                UserName = user.UserName,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CitizenIdentityNumber = user.CitizenIdentityNumber,
            };
        }
        public static User ToEntity(this UserDTO userDto)
        {
            return new User
            {
                UserName = userDto.UserName,
                FullName = userDto.FullName,
                DateOfBirth = userDto.DateOfBirth,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                CitizenIdentityNumber = userDto.CitizenIdentityNumber,
            };
        }
        public static UserResponseDTO ToResponseDTO(this UserDTO user)
        {
            return new UserResponseDTO
            {
                UserName = user.UserName,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}

using RealTimeChat.DTO.UserDTOs;
using RealTimeChat.Models;

namespace RealTimeChat.Interface
{
    public interface IUserRepository
    {
        public Task<User> CreateUserAsync(CreateUserDTO createUserDTO);
        public Task<User?> GetUserByUsernameAsync(string username, UserLoginPasswordDTO userPassword);
        public Task<User?> UpdatePasswordAsync(string username, UpdateUserDTO updateUserDTO);
    }
}

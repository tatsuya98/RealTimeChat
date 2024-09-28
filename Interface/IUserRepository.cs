using RealTimeChat.DTO.UserDTOs;
using RealTimeChat.Models;

namespace RealTimeChat.Interface
{
    public interface IUserRepository
    {
        public Task<User> CreateUserAsync(CreateUserDTO createUserDTO);
        public Task<User?> GetUserByUsernameAsync(string username);
        public Task<User?> UpdatePasswordAsync(string username, UpdateUserDTO updateUserDTO);
    }
}

using RealTimeChat.DTO.UserDTOs;
using RealTimeChat.Models;

namespace RealTimeChat.Mappers
{
    public static class UserMapper
    {
        public static UserDTO ToUserDto(this User user)
        {
            return new UserDTO
            {
                Username = user.Username,
            };
        }
    }
}

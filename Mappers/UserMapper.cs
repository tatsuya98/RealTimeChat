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
                DirectMessageDocuments = user.DirectMessageDocuments,
                GroupChatDocuments = user.GroupChatDocuments,
            };
        }

        public static UserLoginDTO ToUserLoginDTOFromCreateUserDTO(this CreateUserDTO createUserDTO) 
        {
            return new UserLoginDTO
            {
                Password = createUserDTO.Password,
            };
        }
    }
}

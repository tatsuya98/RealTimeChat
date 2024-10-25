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
                ConversationInfo = user.ConversationInfo,
                GroupChatInfo = user.GroupChatInfo,
            };
        }

        public static UserDirectMessageConnectionDTO ToUserDirectMessageDTO(this User user, string connectionId)
        {
            return new UserDirectMessageConnectionDTO
            {
                Username = user.Username,
                ConnecctionId = connectionId
            };
        }
    }
}

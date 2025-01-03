using RealTimeChat.DTO.GroupChatDTOs;
using RealTimeChat.Models;

namespace RealTimeChat.Mappers
{
    public static class ChatRoomMapper
    {
        public static GroupChat ToGroupChatFromCreateGroupChatDTO (this CreateGroupChatDTO createChatRoomDTO )
        {
            return new GroupChat()
            {
                RoomName = createChatRoomDTO.RoomName,
                UsersToAdd = createChatRoomDTO.UsersToAdd,
                IsPublic = createChatRoomDTO.IsPublic,
            };
        }
    }
}

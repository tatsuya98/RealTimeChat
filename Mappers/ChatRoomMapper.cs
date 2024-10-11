using RealTimeChat.DTO.GroupChatDTOs;
using RealTimeChat.Models;

namespace RealTimeChat.Mappers
{
    public static class ChatRoomMapper
    {
        public static ChatRoomDTO ToChatRoomDTOFromChatRoom(this ChatRoom chatRoom)
        {
            return new ChatRoomDTO()
            {
                RoomName = chatRoom.RoomName,
                Users = chatRoom.Users,
                MessageHistory = chatRoom.MessageHistory,
            };
        }
        public static ChatRoom ToChatRoomFromCreateChatRoomDTO (this CreateGroupChatDTO createChatRoomDTO )
        {
            return new ChatRoom()
            {
                RoomName = createChatRoomDTO.RoomName,
                Users = createChatRoomDTO.Users,
                isPublic = createChatRoomDTO.isPublic,
            };
        }
    }
}

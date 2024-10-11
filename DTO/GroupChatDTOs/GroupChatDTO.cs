using RealTimeChat.DTO.MessageDTOs;

namespace RealTimeChat.DTO.GroupChatDTOs
{
    public class ChatRoomDTO
    {
        public string RoomName { get; set; }
        public List<string> Users { get; set; }
        public List<HistotryMessageDTO> MessageHistory { get; set; }
    }
}

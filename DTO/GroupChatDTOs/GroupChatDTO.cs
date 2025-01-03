using RealTimeChat.DTO.MessageDTOs;

namespace RealTimeChat.DTO.GroupChatDTOs
{
    public class GroupChatDTO
    {
        public string RoomName { get; set; } = String.Empty;
        public List<string> UsersToAdd { get; set; } = [];
        public List<HistoryMessageDTO> Messages { get; set; } = [];
    }
}

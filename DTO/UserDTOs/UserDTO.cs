using RealTimeChat.Models;

namespace RealTimeChat.DTO.UserDTOs
{
    public class UserDTO
    {
        public string Username { get; set; } = string.Empty;
        public int MessagesReceived { get; set; } = 0;
        public required List<Dictionary<string, Object>> DirectMessageDocuments { get; set; }
        public required List<Dictionary<string, Object>> GroupChatDocuments { get; set; }
    }
}

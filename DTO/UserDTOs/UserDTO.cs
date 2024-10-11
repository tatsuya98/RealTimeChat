using RealTimeChat.Models;

namespace RealTimeChat.DTO.UserDTOs
{
    public class UserDTO
    {
        public string Username { get; set; }
        public List<Dictionary<string, string>> ConversationInfo { get; set; }
        public List<Dictionary<string, string>> GroupChatInfo { get; set; }
    }
}

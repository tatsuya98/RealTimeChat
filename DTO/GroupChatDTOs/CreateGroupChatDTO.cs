

namespace RealTimeChat.DTO.GroupChatDTOs
{
    public class CreateGroupChatDTO
    {
        public string RoomName { get; set; } = String.Empty;
        public List<string> UsersToAdd { get; set; } = new List<string>();
        public bool IsPublic { get; set; } = true;
    }
}

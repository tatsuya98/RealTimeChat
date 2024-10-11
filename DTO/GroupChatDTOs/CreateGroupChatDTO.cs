

namespace RealTimeChat.DTO.GroupChatDTOs
{
    public class CreateGroupChatDTO
    {
        public string RoomName { get; set; }
        public List<string> Users { get; set; }
        public bool isPublic { get; set; } = false;
    }
}

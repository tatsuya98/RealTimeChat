using RealTimeChat.DTO.MessageDTOs;

namespace RealTimeChat.Models
{
    public class Conversation
    {
        public string Id { get; set; } = new Guid().ToString();
        public string UserId { get; set; }
        public string OtherUserId { get; set; }
        public List<StoreMessageIdDTO> MessageIds { get; set; }
    }
}

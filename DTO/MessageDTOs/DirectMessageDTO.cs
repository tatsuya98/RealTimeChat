namespace RealTimeChat.DTO.MessageDTOs
{
    public class DirectMessageDTO
    {
        
        public string Content { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string Recipient { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
    }
}

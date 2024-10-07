namespace RealTimeChat.DTO.MessageDTOs
{
    public class CreateMessageDTO
    {
        public string Content { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

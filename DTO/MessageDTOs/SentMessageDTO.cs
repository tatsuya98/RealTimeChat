using System.Text.Json.Serialization;

namespace RealTimeChat.DTO.MessageDTOs
{
    public class SentMessageDTO
    {
        [JsonPropertyName("DocumentId")]
        public string DocumentId { get; set; } = string.Empty;
        [JsonPropertyName("Sender")]
        public string Sender { get; set; } = string.Empty;
        [JsonPropertyName("Content")]
        public string Content { get; set; } = string.Empty;
        [JsonPropertyName("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}


using System.Text.Json.Serialization;
using Google.Cloud.Firestore;


namespace RealTimeChat.DTO.MessageDTOs
{
    [FirestoreData]
    public class HistoryMessageDTO
    {
        [FirestoreProperty, JsonPropertyName("Content")]
        public string Content { get; set; } = string.Empty;
        [FirestoreProperty, JsonPropertyName("Sender")]
        public string Sender { get; set; } = string.Empty;
        [FirestoreProperty, JsonPropertyName("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
    }
}


using Google.Cloud.Firestore;


namespace RealTimeChat.DTO.MessageDTOs
{
    [FirestoreData]
    public class HistotryMessageDTO
    {
        [FirestoreProperty]
        public string Content { get; set; }
        [FirestoreProperty]
        public string Sender { get; set; }
        [FirestoreProperty]
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
    }
}

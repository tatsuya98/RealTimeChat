using Google.Cloud.Firestore;

namespace RealTimeChat.Models
{
    [FirestoreData]
    public class Message
    {
        [FirestoreProperty]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [FirestoreProperty]
        public string Content { get; set; } = string.Empty;
        [FirestoreProperty]
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
        [FirestoreProperty]
        public string Sender { get; set; } = string.Empty;
        [FirestoreProperty]
        public string Recipient { get; set; } = string.Empty;
    }
}

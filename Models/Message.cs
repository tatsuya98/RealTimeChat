using Google.Cloud.Firestore;

namespace RealTimeChat.Models
{
    [FirestoreData]
    public class Message
    {
        [FirestoreProperty]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [FirestoreProperty]
        public string Content { get; set; }
        [FirestoreProperty]
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
        [FirestoreProperty]
        public string Sender { get; set; }
        [FirestoreProperty]
        public string Recipient { get; set; }
    }
}

using Google.Cloud.Firestore;

namespace RealTimeChat.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty]
        public string Username { get; set; }
        [FirestoreProperty]
        public string HashedPassword { get; set; }
        [FirestoreProperty]
        public string Salt { get; set; }
        [FirestoreProperty]
        public List<Message> Messages { get; set; }
        [FirestoreProperty]
        public List<string> ConversationDocumentIds { get; set; }

    }
}

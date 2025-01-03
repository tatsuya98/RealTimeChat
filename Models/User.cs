using Google.Cloud.Firestore;

namespace RealTimeChat.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty]
        public string ConnectionId { get; set; } = string.Empty;
        [FirestoreProperty]
        public string Username { get; set; } = string.Empty;
        [FirestoreProperty]
        public string HashedPassword { get; set; } = string.Empty;
        [FirestoreProperty]
        public string Salt { get; set; } = string.Empty;
        [FirestoreProperty]
        public List<Dictionary<string, Object>> DirectMessageDocuments { get; set; } = new List<Dictionary<string, Object>>();
        [FirestoreProperty]
        public List <Dictionary<string, Object>> GroupChatDocuments { get; set;} = new List<Dictionary<string, Object>>();

    }
}

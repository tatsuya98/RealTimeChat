using Google.Cloud.Firestore;

namespace RealTimeChat.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty]
        public string Username { get; set; } = string.Empty;
        [FirestoreProperty]
        public string HashedPassword { get; set; } = string.Empty;
        [FirestoreProperty]
        public string Salt { get; set; } = string.Empty;
        [FirestoreProperty]
        public List<Dictionary<string, string>> ConversationInfo { get; set; } = new List<Dictionary<string, string>>();
        [FirestoreProperty]
        public List <Dictionary<string, string>> GroupChatInfo { get; set;} = new List<Dictionary<string, string>>();

    }
}

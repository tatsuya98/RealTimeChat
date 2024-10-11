using Google.Cloud.Firestore;
using RealTimeChat.DTO.MessageDTOs;

namespace RealTimeChat.Models
{
    [FirestoreData]
    public class ChatRoom
    {
        [FirestoreProperty]
        public string RoomName { get; set; }
        [FirestoreProperty]
        public List<string> Users { get; set; }
        [FirestoreProperty]
        public bool isPublic { get; set; } = false;
        [FirestoreProperty]
        public List<HistotryMessageDTO> MessageHistory { get; set; } = new List<HistotryMessageDTO>();
    }
}

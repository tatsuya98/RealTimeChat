using Google.Cloud.Firestore;
using RealTimeChat.DTO.MessageDTOs;

namespace RealTimeChat.Models
{
    [FirestoreData]
    public class GroupChat
    {
        [FirestoreProperty] public string RoomName { get; set; } = string.Empty;
        [FirestoreProperty] public List<string> UsersToAdd { get; set; } = [];
        [FirestoreProperty] public bool IsPublic { get; set; } = true;
        [FirestoreProperty]
        public List<HistoryMessageDTO> Messages { get; set; } = new List<HistoryMessageDTO>();
    }
}

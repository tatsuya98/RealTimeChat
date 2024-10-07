using Google.Cloud.Firestore;
using RealTimeChat.DTO.MessageDTOs;
using RealTimeChat.Utils;

namespace RealTimeChat.Models
{
    [FirestoreData]
    public class Conversation
    {
        [FirestoreProperty]
        public List<HistotryMessageDTO> Messages { get; set; } = new List<HistotryMessageDTO>();
    }
}

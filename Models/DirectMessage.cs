using Google.Cloud.Firestore;
using RealTimeChat.DTO.MessageDTOs;

namespace RealTimeChat.Models
{
    [FirestoreData]
    public class DirectMessage
    {
        [FirestoreProperty]
        public List<HistotryMessageDTO> MessageHistory { get; set; } = new List<HistotryMessageDTO>();
    }
}

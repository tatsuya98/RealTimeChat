using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Data;
using RealTimeChat.DTO.MessageDTOs;
using RealTimeChat.Interface;
using RealTimeChat.Mappers;
using RealTimeChat.Models;

namespace RealTimeChat.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly FirestoreDb db = FirestoreDatabase.CreateFireStoreInstance();

        [HttpPost]
        public async Task<Message> CreateMessageAsync(CreateMessageDTO createMessageDTO)
        {
            Message message = createMessageDTO.ToMessageFromCreateMessageDTO();
            CollectionReference collection = db.Collection("messages");
            await collection.AddAsync(message);
            return message;
        }
        [HttpDelete]
        public async Task<Message> DeleteMessageAsync(string id)
        {
            Query query = db.Collection("messages").WhereEqualTo("Id", id);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            DocumentReference document = querySnapshot[0].Reference;
            await document.DeleteAsync();
            return new Message();
        }

        public async Task<List<MessageDTO>> GetMessagesByUsernameAsync(string username)
        {
            List<MessageDTO> messageDTOs = new List<MessageDTO>();
            Query query = db.Collection("messages").WhereEqualTo("Username", username);
            QuerySnapshot userMessages = await query.GetSnapshotAsync();
            foreach (DocumentSnapshot document in userMessages.Documents)
            {
                Message data = document.ConvertTo<Message>();
                MessageDTO messageDTO = new() { Content = data.Content, CreatedAt = data.CreatedAt };
                messageDTOs.Add(messageDTO);
            }
            return messageDTOs;
        }
    }
}

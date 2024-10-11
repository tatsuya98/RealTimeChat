using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Data;
using RealTimeChat.DTO.MessageDTOs;
using RealTimeChat.Interface;
using RealTimeChat.Mappers;
using RealTimeChat.Models;

namespace RealTimeChat.Repository
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly FirestoreDb db = FirestoreDatabase.CreateFireStoreInstance();

        [HttpGet]
        public async Task<List<HistotryMessageDTO>?> GetConversationHistory(string conversationDocumentId)
        {
            DocumentReference conversationDocument = db.Collection("Conversations").Document(conversationDocumentId);
            DocumentSnapshot documentSnapshot = await conversationDocument.GetSnapshotAsync();
            if(!documentSnapshot.Exists)
                return null;
            List<HistotryMessageDTO> ConversationHistory = documentSnapshot.GetValue<List<HistotryMessageDTO>>("MessageHistory");
            ConversationHistory = ConversationHistory.OrderByDescending(message => message.CreatedAt).ToList();
            return ConversationHistory;
        }

        [HttpPost]
        public async Task<string> CreateConversationHistory(CreateMessageDTO messageDTO)
        {
            List<HistotryMessageDTO> messageHistoryList = new List<HistotryMessageDTO>();
            DirectMessage newMessageHistory = new DirectMessage();
            HistotryMessageDTO messageToStore = messageDTO.ToHistoryMesssageDTOFromCreateMessageDTO();
            newMessageHistory.MessageHistory.Add(messageToStore);
            CollectionReference collection = db.Collection("Conversations");
            DocumentReference document = await collection.AddAsync(newMessageHistory);
            List<string> Usernames = [messageDTO.Sender,  messageDTO.Recipient];
            Dictionary<string, string> documentInfoToBeStored = new Dictionary<string, string>
            {
                {"documentId", document.Id },
                {"Sender", messageDTO.Sender },
                {"Recipient", messageDTO.Recipient },
            };
             Utils.Utils.StoreDocumentInfoInUser(db, Usernames, documentInfoToBeStored, "ConversationInfo");
            return document.Id;
        }

        [HttpPut]
        public async Task<HistotryMessageDTO?> StoreMessageInConversationHistory(string conversationDocumentId, HistotryMessageDTO messageToAdd)
        {
            
            DocumentReference document = db.Collection("Conversations").Document(conversationDocumentId);
            DocumentSnapshot documentSnapshot = await document.GetSnapshotAsync();
            if (!documentSnapshot.Exists)
                return null;
            List<HistotryMessageDTO> messageHistory = documentSnapshot.GetValue<List<HistotryMessageDTO>>("MessageHistory");
            messageHistory.Add(messageToAdd);
            Dictionary<FieldPath, object> update = new Dictionary<FieldPath, object>
            {
                 { new FieldPath("Messages"), messageHistory}
            };
            await document.UpdateAsync(update);
            return messageToAdd;
        }
    }
}

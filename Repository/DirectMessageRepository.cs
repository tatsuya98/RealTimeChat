using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Data;
using RealTimeChat.DTO.MessageDTOs;
using RealTimeChat.Interface;
using RealTimeChat.Mappers;



namespace RealTimeChat.Repository
{
    public class DirectMessageRepository : IDirectMessageRepository
    {
        private readonly FirestoreDb _db = FirestoreDatabase.CreateFireStoreInstance();

        [HttpGet]
        public async Task<List<HistoryMessageDTO>?> GetDirectMessageHistory(string conversationDocumentId)
        {
            DocumentReference conversationDocument = _db.Collection("DirectMessage").Document(conversationDocumentId);
            DocumentSnapshot documentSnapshot = await conversationDocument.GetSnapshotAsync();
            if(!documentSnapshot.Exists)
                return null;
            List<HistoryMessageDTO> conversationHistory = documentSnapshot.GetValue<List<HistoryMessageDTO>>("Messages");
            conversationHistory = conversationHistory.OrderByDescending(message => message.CreatedAt).ToList();
            return conversationHistory;
        }
        public async Task<string> StoreMessageInDirectMessageHistory(string documentId, DirectMessageDTO directMessageData)
        {
            HistoryMessageDTO messageToStore = directMessageData.ToHistoryMesssageDTOFromMessageDTO();
            Dictionary<string, Object> documentInfoToBeStored = new Dictionary<string, Object>
            {
                {"DocumentId", "" },
                {"Recipient", directMessageData.Recipient },
                {"MessagesReceived", 0}
            };
            List<string> users = [directMessageData.Sender, directMessageData.Recipient];
            bool docIdExistsInAll = await Utils.Utils.DocumentIdExistsInUsers(_db, documentId, users, "DirectMessageDocuments");
            string transactionResult = await _db.RunTransactionAsync(async transaction =>
            {

                if (!docIdExistsInAll)
                {
                    DocumentReference document = _db.Collection("DirectMessage").Document();
                    transaction.Create(document, new Dictionary<string, object>()
                    {
                        {"Messages", new List<HistoryMessageDTO>{ messageToStore } }
                    });
                    documentInfoToBeStored["DocumentId"] = document.Id;

                    await Utils.Utils.StoreDocumentInfoInUsers(_db, documentInfoToBeStored, "DirectMessageDocuments", users);
                    return document.Id;
                }
                else
                {
                    DocumentReference document = _db.Collection("DirectMessage").Document(documentId);
                    DocumentSnapshot documentSnapshot = await document.GetSnapshotAsync();
                    List<HistoryMessageDTO> existingMessageHistory = documentSnapshot.GetValue<List<HistoryMessageDTO>>("Messages");
                    existingMessageHistory.Add(messageToStore);
                    Dictionary<string, object> update = new Dictionary<string, object>
                    {
                            { "Messages", existingMessageHistory}
                    };
                    documentInfoToBeStored["documentId"] = document.Id;
                    await document.UpdateAsync(update);
                    return document.Id;
                }
            });
            return transactionResult;
        }
        
        public async Task<string> HandleMessagesReceived(string documentId, string username, string documentType)
        {
            await Utils.Utils.IncrementMessagesReceived(_db, username, documentId, documentType);
            return documentId;
        }
        
        public async Task<string> HandleMessagesReceivedReset(string documentId, string username, string documentType)
        {
            await Utils.Utils.ResetMessagesReceived(_db, username, documentId, documentType);
            return documentId;
        }
    }
}

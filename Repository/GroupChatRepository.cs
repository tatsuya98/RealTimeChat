using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Data;
using RealTimeChat.DTO.GroupChatDTOs;
using RealTimeChat.DTO.MessageDTOs;
using RealTimeChat.Interface;
using RealTimeChat.Mappers;
using RealTimeChat.Models;


namespace RealTimeChat.Repository
{
    public class GroupChatRepository : IGroupChatRepository
    {
        private readonly FirestoreDb _db = FirestoreDatabase.CreateFireStoreInstance();
        [HttpGet]
        public async Task<List<HistoryMessageDTO>> GetGroupChatMessagesFromDocumentAsync(string documentId)
        {
            DocumentReference document = _db.Collection("GroupChat").Document(documentId);
            DocumentSnapshot documentSnapshot = await document.GetSnapshotAsync();
            List<HistoryMessageDTO> chatrooms = documentSnapshot.GetValue<List<HistoryMessageDTO>>("Messages");
            return chatrooms;
        }
        public async Task<Dictionary<string, Object>> CreateGroupChatDocumentAsync(CreateGroupChatDTO createChatRoomDTO)
        {
            GroupChat newChatRoom = createChatRoomDTO.ToGroupChatFromCreateGroupChatDTO();
            CollectionReference collection = _db.Collection("GroupChat");
            DocumentReference document = await collection.AddAsync(newChatRoom);
            Dictionary<string, Object> documentInfoToStore = new Dictionary<string, Object>
            {
                {"RoomName", createChatRoomDTO.RoomName },
                {"DocumentId", document.Id },
                {"MessagesReceived", 0}
            };

            await Utils.Utils.StoreDocumentInfoInUsers(_db, documentInfoToStore, "GroupChatDocuments", newChatRoom.UsersToAdd);
            return documentInfoToStore;
        }

        public async void UpdateGroupChatMessageHistoryAsync(string documentId, HistoryMessageDTO messageData)
        {
            DocumentReference document = _db.Collection("GroupChat").Document(documentId);
            DocumentSnapshot documentSnapshot = await document.GetSnapshotAsync();
            List<HistoryMessageDTO> existingMessageHistory = documentSnapshot.GetValue<List<HistoryMessageDTO>>("Messages");
            existingMessageHistory.Add(messageData);
            await document.UpdateAsync(new Dictionary<string, object> { { "Messages", existingMessageHistory } });
        }

        [HttpDelete]
        public async Task<string> DeleteGroupChatAsync(string groupChatId)
        {
            DocumentReference groupChatToDelete = _db.Collection("GroupChat").Document(groupChatId);
            DocumentSnapshot documentSnapshot = await groupChatToDelete.GetSnapshotAsync();
            if (!documentSnapshot.Exists)
                throw new Exception("document id is incorrect");
            List<string> users = documentSnapshot.GetValue<List<string>>("Users");
            foreach(string user in users)
            {
                Query query = _db.Collection("Users").WhereEqualTo("Username", user);
                QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
                DocumentReference document = querySnapshot[0].Reference;
                User foundUser = querySnapshot[0].ConvertTo<User>();
                var filtredGroupInfo = foundUser.GroupChatDocuments.Select(x => !x.ContainsValue(groupChatId));
                Dictionary<FieldPath, object> update = new Dictionary<FieldPath, object>
                {
                    {new FieldPath("GroupChatInfo"), filtredGroupInfo }
                };
                await document.UpdateAsync(update);
            }
            await groupChatToDelete.DeleteAsync();
            return groupChatId;
        }
    }
}

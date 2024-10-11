using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Data;
using RealTimeChat.DTO.GroupChatDTOs;
using RealTimeChat.DTO.MessageDTOs;
using RealTimeChat.Interface;
using RealTimeChat.Mappers;
using RealTimeChat.Models;
using RealTimeChat.Utils;


namespace RealTimeChat.Repository
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        private readonly FirestoreDb db = FirestoreDatabase.CreateFireStoreInstance();
        [HttpGet]
        public async Task<List<Dictionary<string, string>>?> GetChatRoomsFromUserDocumentAsync(string username)
        {
            Query query = db.Collection("Users").WhereEqualTo("Username", username);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            if(querySnapshot.Count == 0)
            {
                return null;
            }
            DocumentSnapshot documentSnapshot = querySnapshot[0];
            List<Dictionary<string,string>> chatrooms = documentSnapshot.GetValue<List<Dictionary<string, string>>>("GroupChatInfo");
            return chatrooms;
        }
        [HttpPost]
        public async Task<Dictionary<string, string>> CreateChatRoomDocumentAsync(CreateGroupChatDTO createChatRoomDTO)
        {
            bool usersExist = await Utils.Utils.UserExists(db, createChatRoomDTO.Users);
            if (!usersExist)
                throw new Exception("users not found");
            ChatRoom newChatRoom = createChatRoomDTO.ToChatRoomFromCreateChatRoomDTO();
            CollectionReference collection = db.Collection("GroupChat");
            DocumentReference document = await collection.AddAsync(newChatRoom);
            Dictionary<string, string> documentInfoToStore = new Dictionary<string, string>
            {
                {"roomName", createChatRoomDTO.RoomName },
                {"documentId", document.Id }
            };

            Utils.Utils.StoreDocumentInfoInUser(db, newChatRoom.Users, documentInfoToStore, "GroupChatInfo");
            return documentInfoToStore;
        }
        [HttpDelete]
        public async Task<string> DeleteChatRoomAsync(string groupChatId)
        {
            DocumentReference groupChatToDelete = db.Collection("GroupChat").Document(groupChatId);
            DocumentSnapshot documentSnapshot = await groupChatToDelete.GetSnapshotAsync();
            if (!documentSnapshot.Exists)
                throw new Exception("document id is incorrect");
            List<string> users = documentSnapshot.GetValue<List<string>>("Users");
            foreach(string user in users)
            {
                Query query = db.Collection("Users").WhereEqualTo("Username", user);
                QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
                DocumentReference document = querySnapshot[0].Reference;
                User foundUser = querySnapshot[0].ConvertTo<User>();
                var filtredGroupInfo = foundUser.GroupChatInfo.Select(x => !x.ContainsValue(groupChatId));
                Dictionary<FieldPath, object> update = new Dictionary<FieldPath, object>
                {
                    {new FieldPath("GroupChatInfo"), filtredGroupInfo }
                };
                await document.UpdateAsync(update);
            }
            await groupChatToDelete.DeleteAsync();
            return groupChatId;
        }
        [HttpPut]
        public async Task<string> UpdateChatRoomMessageHistoryAsync(HistotryMessageDTO histotryMessageDTO, string documentId)
        {
            DocumentReference document = db.Collection("GroupChat").Document(documentId);
            DocumentSnapshot groupChatSnapshot = await document.GetSnapshotAsync();
            List<HistotryMessageDTO> messageHistoryToUpdate = groupChatSnapshot.GetValue<List<HistotryMessageDTO>>("MessageHistory");
            messageHistoryToUpdate.Add(histotryMessageDTO);
            Dictionary<FieldPath, object> update = new Dictionary<FieldPath, object>
            {
                {new FieldPath("MessageHistory"), messageHistoryToUpdate },
            };
            await document.UpdateAsync(update);
            return documentId;
        }
    }
}

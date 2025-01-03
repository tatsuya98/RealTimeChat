using System.Collections.Concurrent;
using Google.Cloud.Firestore;

namespace RealTimeChat.Utils
{
    public static class Utils
    {
        public static (string, string) HashPassword(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return (hashedPassword, salt);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public static async Task<string> StoreDocumentInfoInUsers(FirestoreDb db, Dictionary<string, Object> documentInfoToBeStored, string fieldName, List<string>? usernames)
        {
            Query query = db.Collection("Users").WhereIn("Username", usernames);
            QuerySnapshot usersSnapshots = await query.GetSnapshotAsync();
            string recipient = usernames![1];
            foreach (DocumentSnapshot userDocumentSnapshot in usersSnapshots)
            {
                DocumentReference userDocument = userDocumentSnapshot.Reference;
                string username = userDocumentSnapshot.GetValue<string>("Username");
                if(username == recipient)
                {
                    documentInfoToBeStored["Recipient"] = usernames.First();
                }
                List<Dictionary<string, Object>> userDocumentInfo = userDocumentSnapshot.GetValue<List<Dictionary<string, Object>>>(fieldName);
                userDocumentInfo.Add(documentInfoToBeStored);
                Dictionary<FieldPath, object> update = new Dictionary<FieldPath, object>
                {
                    {new FieldPath(fieldName), userDocumentInfo }
                };
                await userDocument.UpdateAsync(update);
            }
            return recipient;
        }

        public static async Task<bool> DocumentIdExistsInUsers(FirestoreDb db, string documentId, List<string> usernames, string fieldname)
        {
            Query query = db.Collection("Users").WhereIn("Username", usernames);
            QuerySnapshot querySnapshots = await query.GetSnapshotAsync();
            List<Dictionary<string, Object>> userDocumentsToCheck = new List<Dictionary<string, Object>>();
            foreach (DocumentSnapshot userDocumentSnapshot in querySnapshots)
            {
                List<Dictionary<string, Object>> userDirectMessageDocuments = userDocumentSnapshot.GetValue<List<Dictionary<string, Object>>>(fieldname);
                userDocumentsToCheck = userDocumentsToCheck.Concat(userDirectMessageDocuments).ToList();
            }
            if(userDocumentsToCheck.Count == 0)
            {
                return false;
            }
            return userDocumentsToCheck.TrueForAll(doc => doc["DocumentId"].ToString() == documentId);
        }
        
        public static void StoreConnectedUsersId(ConcurrentDictionary<string, string> connectedUsers,string username, string connectionId)
        {
            connectedUsers.AddOrUpdate(username, connectionId, (key, oldValue) => connectionId);
        }
        public static async  Task<string> IncrementMessagesReceived( FirestoreDb db, string username, string documentId, string documentType)
        {
            Query query = db.Collection("Users").WhereEqualTo("Username", username);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            DocumentReference document = querySnapshot[0].Reference;
            DocumentSnapshot documentSnapshot = await document.GetSnapshotAsync();
            List<Dictionary<string, Object>> documents = new ();
            Dictionary<FieldPath, object> update = new Dictionary<FieldPath, object>();
            if (documentType == "directMessage")
            {
                documents = documentSnapshot.GetValue<List<Dictionary<string, Object>>>("DirectMessageDocuments");
                Dictionary<string, Object> updatedDocument = documents.Find(x => x["DocumentId"].ToString() == documentId);
                updatedDocument["MessagesReceived"] = int.Parse(updatedDocument["MessagesReceived"].ToString()) + 1;
                int index = documents.FindIndex((x) => x["DocumentId"].ToString() == documentId);
                documents[index] = updatedDocument;
                update = new Dictionary<FieldPath, object>
                {
                    {new FieldPath("DirectMessageDocuments"), documents }
                };
                
            }
            else
            {
                documents = documentSnapshot.GetValue<List<Dictionary<string, Object>>>("GroupChatDocuments");
                Dictionary<string, Object> updatedDocument = documents.Find(x => x["DocumentId"].ToString() == documentId);
                updatedDocument["MessagesReceived"] = int.Parse(updatedDocument["MessagesReceived"].ToString()) + 1;
                int index = documents.FindIndex((x) => x["DocumentId"].ToString() == documentId);
                documents[index] = updatedDocument;
                update = new Dictionary<FieldPath, object>
                {
                    {new FieldPath("GroupChatDocuments"), documents }
                };
            }

            
            await document.UpdateAsync(update);
            return document.Id;
        }

        public static async Task<string> ResetMessagesReceived(FirestoreDb db, string username, string documentId, string documentType)
        {
            Query query = db.Collection("Users").WhereEqualTo("Username", username);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            DocumentReference document = querySnapshot[0].Reference;
            DocumentSnapshot documentSnapshot = await document.GetSnapshotAsync();
            List<Dictionary<string, Object>> documents = new ();
            Dictionary<FieldPath, object> update = new Dictionary<FieldPath, object>();
            if (documentType == "directMessage")
            {
                documents = documentSnapshot.GetValue<List<Dictionary<string, Object>>>("DirectMessageDocuments");
                Dictionary<string, Object> updatedDocument = documents.Find(x => x["DocumentId"].ToString() == documentId);
                updatedDocument["MessagesReceived"] = 0;
                int index = documents.FindIndex((x) => x["DocumentId"].ToString() == documentId);
                documents[index] = updatedDocument;
                update = new Dictionary<FieldPath, object>
                {
                    {new FieldPath("DirectMessageDocuments"), documents }
                };
                
            }
            else
            {
                documents = documentSnapshot.GetValue<List<Dictionary<string, Object>>>("GroupChatDocuments");
                Dictionary<string, Object> updatedDocument = documents.Find(x => x["DocumentId"].ToString() == documentId);
                updatedDocument["MessagesReceived"] = 0;
                int index = documents.FindIndex((x) => x["DocumentId"].ToString() == documentId);
                documents[index] = updatedDocument;
                update = new Dictionary<FieldPath, object>
                {
                    {new FieldPath("GroupChatDocuments"), documents }
                };
            }

            
            await document.UpdateAsync(update);
            return document.Id;
        }
    }
}

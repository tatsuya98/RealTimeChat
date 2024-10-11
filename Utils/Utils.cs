using Google.Cloud.Firestore;
using RealTimeChat.DTO.MessageDTOs;

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

        public async static void StoreDocumentInfoInUser(FirestoreDb db, List<string> usernames, Dictionary<string, string> documentInfo, string fieldName)
        {
            Query query = db.Collection("Users").WhereIn("Username", usernames);
            QuerySnapshot usersSnapshot = await query.GetSnapshotAsync();
            foreach (DocumentSnapshot userSnapshot in usersSnapshot)
            {
                DocumentReference userDocument = userSnapshot.Reference;
                List<Dictionary<string, string>> userDocumentInfo = userSnapshot.GetValue<List<Dictionary<string, string>>>(fieldName);
                userDocumentInfo.Add(documentInfo);
                Dictionary<FieldPath, object> update = new Dictionary<FieldPath, object>
                {
                    {new FieldPath(fieldName), userDocumentInfo }
                };
                await userDocument.UpdateAsync(update);
            }
        }

        public async static Task<bool> UserExists(FirestoreDb db, List<string> usernames)
        {
            Query query = db.Collection("Users").WhereIn("Username", usernames);
            QuerySnapshot usersSnapshot = await query.GetSnapshotAsync();
            if (usersSnapshot.Count == 0)
                return false;
            return true;
        }
    }
}

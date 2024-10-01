using Google.Cloud.Firestore;

namespace RealTimeChat.Data
{
    public static class FirestoreDatabase
    {
        public static FirestoreDb CreateFireStoreInstance()
        {
            return FirestoreDb.Create("realtimechat-8a931");
        }
    }
}

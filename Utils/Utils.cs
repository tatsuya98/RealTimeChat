using Google.Cloud.Firestore;

namespace RealTimeChat.Utils
{
    public static class Utils
    {
        public static  (string, string)  HashPassword(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return (hashedPassword, salt);
        }
    }
}

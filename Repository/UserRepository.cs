using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Data;
using RealTimeChat.DTO.UserDTOs;
using RealTimeChat.Interface;
using RealTimeChat.Models;


namespace RealTimeChat.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly FirestoreDb _db = FirestoreDatabase.CreateFireStoreInstance();
        [HttpGet]
        public async Task<UserSearchInfoDTO> GetUserInfoByUsername(string username)
        {
            CollectionReference collection = _db.Collection("Users");
            QuerySnapshot querySnapshot = await collection.WhereEqualTo("Username", username).GetSnapshotAsync();
            UserSearchInfoDTO user = new();
            if (querySnapshot.Count == 1)
            {
                DocumentSnapshot documentSnapshot = querySnapshot.First();
                
                user.Username = documentSnapshot.GetValue<string>("Username");
            }
            return user;
        }
        [HttpPost]
        public async Task<User> CreateUserAsync(CreateUserDTO createUserDTO)
        {
            CollectionReference collection = _db.Collection("Users");
            QuerySnapshot querySnapshot = await collection.WhereEqualTo("Username", createUserDTO.Username).GetSnapshotAsync();
            if (querySnapshot.Count == 1)
                throw new InvalidOperationException("username already exists");

            User user = new();
            (string, string) hashData = Utils.Utils.HashPassword(createUserDTO.Password);
            user.Username = createUserDTO.Username;
            user.HashedPassword = hashData.Item1;
            user.Salt = hashData.Item2;
            await collection.AddAsync(user);
            return user;
        }
        [HttpPost("{username}")]
        public async Task<User?> GetUserByUsernameAsync(string username, UserLoginDTO userLoginData)
        {
            Query collection = _db.Collection("Users").WhereEqualTo("Username", username);
            QuerySnapshot querySnapshot = await collection.GetSnapshotAsync();
            if (querySnapshot.Count == 0)
            {
                return null;
            }
            DocumentSnapshot userDocumentSnapshot = querySnapshot.Documents.First();
            User user = querySnapshot[0].ConvertTo<User>();
            bool isPasswordCorrect = Utils.Utils.VerifyPassword(userLoginData.Password, user.HashedPassword);
            if (!isPasswordCorrect)
            {
                throw new Exception("Incorrect Password");
            }
            return user;
        }
        [HttpPut]
        public async Task<User?> UpdatePasswordAsync(string username, UpdateUserDTO updateUserDTO)
        {

            (string, string) hashData = Utils.Utils.HashPassword(updateUserDTO.Password);
            Query collection = _db.Collection("Users").WhereEqualTo("Username", username);
            QuerySnapshot querySnapshot = await collection.GetSnapshotAsync();
            if (querySnapshot.Count == 0)
            {
                return null;
            }
            DocumentReference document = querySnapshot[0].Reference;
            Dictionary<FieldPath, object> updatates = new Dictionary<FieldPath, object>
            {
                {new FieldPath("HashedPassword"), hashData.Item1 },
                {new FieldPath("Salt"), hashData.Item2 }
            };
            await document.UpdateAsync(updatates);
            return querySnapshot[0].ConvertTo<User>();
        }
    }
}

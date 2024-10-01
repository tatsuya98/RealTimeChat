using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Data;
using RealTimeChat.DTO.UserDTOs;
using RealTimeChat.Interface;
using RealTimeChat.Models;
using RealTimeChat.Utils;

namespace RealTimeChat.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly FirestoreDb _db = FirestoreDatabase.CreateFireStoreInstance();
        [HttpPost]
        public async Task<User> CreateUserAsync(CreateUserDTO createUserDTO)
        {
            CollectionReference collection = _db.Collection("users");
            QuerySnapshot querySnapshot = await collection.WhereEqualTo("Username", createUserDTO.Username).GetSnapshotAsync();
            Console.WriteLine(querySnapshot.Count);
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
        [HttpGet]
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            Query collection = _db.Collection("users").WhereEqualTo("Username", username);
            QuerySnapshot querySnapshot = await collection.GetSnapshotAsync();
            if(querySnapshot.Count < 1)
            {
                return null;
            }
            User user = querySnapshot[0].ConvertTo<User>();
            return user;
        }
        [HttpPut]
        public async Task<User?> UpdatePasswordAsync(string username, UpdateUserDTO updateUserDTO)
        {
            (string, string) hashData = Utils.Utils.HashPassword(updateUserDTO.Password);
            Query collection = _db.Collection("users").WhereEqualTo("Username", username);
            QuerySnapshot querySnapshot = await collection.GetSnapshotAsync();
            if (querySnapshot.Count < 1)
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

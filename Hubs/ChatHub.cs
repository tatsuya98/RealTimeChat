using System.Collections.Concurrent;
using RealTimeChat.DTO.GroupChatDTOs;

namespace RealTimeChat.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using DTO.MessageDTOs;
    using Interface;
    using Mappers;


    public class ChatHub(IDirectMessageRepository directMessageRepo, IGroupChatRepository groupChatRepository, ConcurrentDictionary<string, string> connectedUsers) : Hub
    {
        private readonly IDirectMessageRepository _directMessageRepo = directMessageRepo;
        private readonly IGroupChatRepository _groupChatRepository = groupChatRepository;

        private readonly ConcurrentDictionary<string, string> _connectedUsers = connectedUsers;
       
        public void UpdateReconnectedUser(string username = "")
        {
            if (!String.IsNullOrEmpty(username))
            {
                
                Utils.Utils.StoreConnectedUsersId(_connectedUsers, username, Context.ConnectionId);
            }
        }
        public void UpdateConnectedUsersId(string username)
        {
            
            Utils.Utils.StoreConnectedUsersId(_connectedUsers, username, Context.ConnectionId);
        }
        public void UpdateMessagesReceived(string documentId, string username,  string documentType)
        {
            _directMessageRepo.HandleMessagesReceived(documentId, username, documentType);
        }
        public void ResetMessagesReceived(string documentId, string username,  string documentType)
        {
            _directMessageRepo.HandleMessagesReceivedReset(documentId, username, documentType);
        }
        public async Task NewMessage(string username, string message)
        {
            var messageData = new SentMessageDTO()
            {
                Content = message,
                Sender = username
            };
            await Clients.All.SendAsync("ReceiveMessage", messageData);
        }
        public async Task NewMessageAnonymous(string username, string message)
        {
           var messageData = new SentMessageDTO()
            {
                Content = message,
                Sender = username
            };
            await Clients.All.SendAsync("ReceiveMessage", messageData);
        }
        public async Task SendPrivateMessage(DirectMessageDTO directMessageData, string documentId)
        {
            var recipientConnectionId = _connectedUsers.FirstOrDefault((x) => x.Key == directMessageData.Recipient).Value;
            var newDocumentId = await _directMessageRepo.StoreMessageInDirectMessageHistory(documentId, directMessageData);
            SentMessageDTO messageDataToSend = directMessageData.ToSentMessageDTOFromMessageDTO(newDocumentId);
            await Clients.Client(recipientConnectionId).SendAsync("ReceivePrivateMessage", messageDataToSend);
            await Clients.Caller.SendAsync("SendMessageConfirmation", messageDataToSend);
        }

        public async Task AddAllToGroup(CreateGroupChatDTO groupChatData)
        {
            
            var dataToSend = await _groupChatRepository.CreateGroupChatDocumentAsync(groupChatData);
            foreach (var user in groupChatData.UsersToAdd)
            {
                var connectionId = _connectedUsers.FirstOrDefault(x => x.Key == user).Value;
                await Groups.AddToGroupAsync(connectionId, groupChatData.RoomName);
            }
            await Clients.Group(groupChatData.RoomName).SendAsync("GroupChatCreated", dataToSend);
        }

        public async Task AddUserToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        public async Task SendGroupMessage(HistoryMessageDTO messageData, string documentId, string groupName)
        {
            
             _groupChatRepository.UpdateGroupChatMessageHistoryAsync(documentId, messageData);
             await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", messageData);
        }
    }
}

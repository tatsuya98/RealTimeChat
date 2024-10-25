namespace RealTimeChat.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        public async Task NewMessage(string username, string message) =>
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        public async Task NewMessageAnonymous(long username, string message) =>
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        public Task SendPrivateMessage(string username, string message) =>
            Clients.User(username).SendAsync("ReceiveMessage",message);
        //public async Task AddToGroup()
    }
}

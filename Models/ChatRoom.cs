namespace RealTimeChat.Models
{
    public class ChatRoom
    {
        public string RoomName { get; set; }
        public List<string> Participants { get; set; }
        public bool isPublic { get; set; } = false;
    }
}

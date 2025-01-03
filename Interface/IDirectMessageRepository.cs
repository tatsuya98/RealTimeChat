using RealTimeChat.DTO.MessageDTOs;


namespace RealTimeChat.Interface
{
    public interface IDirectMessageRepository
    {
        public Task<string> StoreMessageInDirectMessageHistory(string conversationDocumentId, DirectMessageDTO directMessageData);
        public Task<List<HistoryMessageDTO>?> GetDirectMessageHistory(string conversationDocumentId);
        public Task<string> HandleMessagesReceived(string documentId, string username, string documentType);
        public Task<string> HandleMessagesReceivedReset(string documentId, string username, string documentType);
    }
}

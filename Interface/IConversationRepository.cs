using RealTimeChat.DTO.MessageDTOs;


namespace RealTimeChat.Interface
{
    public interface IConversationRepository
    {
        public Task<HistotryMessageDTO?> StoreMessageInConversationHistory(string conversationDocumentId, HistotryMessageDTO messageToAdd);
        public Task<string> CreateConversationHistory(CreateMessageDTO messageDTO);
        public Task<List<HistotryMessageDTO>?> GetConversationHistory(string conversationDocumentId);
    }
}

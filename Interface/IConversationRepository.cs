using RealTimeChat.DTO.MessageDTOs;


namespace RealTimeChat.Interface
{
    public interface IConversationRepository
    {
        public Task<CreateMessageDTO?> StoreMessageInConversationHistory(string conversationDocumentId, CreateMessageDTO createMessageDTO);
        public Task<string> CreateConversationHistory(CreateMessageDTO messageDTO);
        public Task<List<HistotryMessageDTO>?> GetConversationHistory(string conversationDocumentId);
    }
}

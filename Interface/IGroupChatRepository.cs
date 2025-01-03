using RealTimeChat.DTO.GroupChatDTOs;
using RealTimeChat.DTO.MessageDTOs;
using RealTimeChat.Models;

namespace RealTimeChat.Interface
{
    public interface IGroupChatRepository
    {
        public Task<List<HistoryMessageDTO>> GetGroupChatMessagesFromDocumentAsync(string documentId);
        public Task<Dictionary<string, Object>> CreateGroupChatDocumentAsync(CreateGroupChatDTO createChatRoomDTO);
        public void UpdateGroupChatMessageHistoryAsync(string documentId, HistoryMessageDTO messageData);
        public Task<string> DeleteGroupChatAsync(string chatroomName);
    }
}

using RealTimeChat.DTO.GroupChatDTOs;
using RealTimeChat.DTO.MessageDTOs;
using RealTimeChat.Models;

namespace RealTimeChat.Interface
{
    public interface IChatRoomRepository
    {
        public Task<List<Dictionary<string, string>>?> GetChatRoomsFromUserDocumentAsync(string username);
        public Task<Dictionary<string, string>> CreateChatRoomDocumentAsync(CreateGroupChatDTO createChatRoomDTO);
        public Task<string> UpdateChatRoomMessageHistoryAsync(HistotryMessageDTO histotryMessageDTO, string documentId);
        public Task<string> DeleteChatRoomAsync(string chatroomName);
    }
}

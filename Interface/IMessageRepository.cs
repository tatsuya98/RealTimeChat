using RealTimeChat.DTO.MessageDTOs;
using RealTimeChat.Models;

namespace RealTimeChat.Interface
{
    public interface IMessageRepository
    {
        public Task<List<MessageDTO>> GetMessagesByUsernameAsync(string username);
        public Task<Message> CreateMessageAsync(CreateMessageDTO createMessageDTO);
        public Task<Message> DeleteMessageAsync(string id);
    }
}

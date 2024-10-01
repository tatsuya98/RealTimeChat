using RealTimeChat.DTO.MessageDTOs;
using RealTimeChat.Models;

namespace RealTimeChat.Mappers
{
    public static class MessageMapper
    {
        public static MessageDTO ToMessageDTO(this Message message)
        {
            return new MessageDTO()
            {
                Content = message.Content,
                CreatedAt = message.CreatedAt,
            };
        }
        public static Message ToMessageFromCreateMessageDTO(this CreateMessageDTO createMessageDTO)
        {
            return new Message()
            {
                Content = createMessageDTO.Content,
                Username = createMessageDTO.Username
            };
        }
    }
}

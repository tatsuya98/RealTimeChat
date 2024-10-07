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
                Id = message.Id,
                Content = message.Content,
                CreatedAt = message.CreatedAt,
            };
        }
        public static Message ToMessageFromCreateMessageDTO(this CreateMessageDTO createMessageDTO)
        {
            return new Message()
            {
                Content = createMessageDTO.Content,
                Sender = createMessageDTO.Sender,
                Recipient = createMessageDTO.Recipient,
            };
        }

        public static HistotryMessageDTO ToHistoryMesssageDTOFromCreateMessageDTO(this CreateMessageDTO createMessageDTO)
        {
            return new HistotryMessageDTO()
            {
                Content = createMessageDTO.Content,
                Sender = createMessageDTO.Sender,
                CreatedAt = createMessageDTO.CreatedAt,
            };
        }
    }
}

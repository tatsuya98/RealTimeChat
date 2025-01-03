using RealTimeChat.DTO.MessageDTOs;
using RealTimeChat.Models;

namespace RealTimeChat.Mappers
{
    public static class MessageMapper
    {
        public static HistoryMessageDTO ToHistoryMesssageDTOFromMessageDTO(this DirectMessageDTO directMessageDto)
        {
            return new HistoryMessageDTO()
            {
                Content = directMessageDto.Content,
                Sender = directMessageDto.Sender,
                CreatedAt = directMessageDto.CreatedAt,
            };
        }

        public static SentMessageDTO ToSentMessageDTOFromMessageDTO(this DirectMessageDTO directMessageDto, string documentId)
        {
            return new SentMessageDTO()
            {
                DocumentId = documentId,
                Content = directMessageDto.Content,
                Sender = directMessageDto.Sender                             
            };
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using RealTimeChat.DTO.GroupChatDTOs;
using RealTimeChat.DTO.MessageDTOs;
using RealTimeChat.Interface;

namespace RealTimeChat.Controllers
{
    [Route("api/GroupChat")]
    [ApiController]
    public class GroupChatController : ControllerBase
    {
        private readonly IGroupChatRepository _repo;
        public GroupChatController(IGroupChatRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        [Route("{documentId}")]
        public async Task<IActionResult> GetGroupChatMessagesByDocumentId([FromRoute]  string documentId)
        {
            List<HistoryMessageDTO> groupChatMessages = await _repo.GetGroupChatMessagesFromDocumentAsync(documentId);
            return Ok(groupChatMessages);
        }
        [HttpDelete]
        [Route("{documentId}")]
        public async Task<IActionResult> DeleteGroupChatRoom([FromRoute] string documentId)
        {
            await _repo.DeleteGroupChatAsync(documentId);
            return NoContent();
        }

    }
}

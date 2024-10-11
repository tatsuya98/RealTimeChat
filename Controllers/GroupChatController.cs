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
        private readonly IChatRoomRepository _repo;
        public GroupChatController(IChatRoomRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        [Route("{username}")]
        public async Task<IActionResult> GetChatRoomsByUsername([FromRoute]  string username)
        {
            List<Dictionary<string, string>>? groupChatRooms = await _repo.GetChatRoomsFromUserDocumentAsync(username);
            if ( groupChatRooms == null)
            {
                return NotFound();
            }
            return Ok(groupChatRooms);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChatRoomInFireStore([FromBody] CreateGroupChatDTO createGroupChatDTO)
        {
            try
            {
                Dictionary<string, string> groupChatInfo = await _repo.CreateChatRoomDocumentAsync(createGroupChatDTO);
                return CreatedAtAction(nameof(CreateChatRoomInFireStore), groupChatInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("{groupChatId}")]
        public async Task<IActionResult> UpdateMessageHistory([FromRoute] string groupChatId, [FromBody] HistotryMessageDTO messageToAdd)
        {
            await _repo.UpdateChatRoomMessageHistoryAsync(messageToAdd, groupChatId);
            return NoContent();
        }
        [HttpDelete]
        [Route("{groupChatId}")]
        public async Task<IActionResult> DeleteGroupChatRoom([FromRoute] string groupChatId)
        {
            await _repo.DeleteChatRoomAsync(groupChatId);
            return NoContent();
        }

    }
}

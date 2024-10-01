using Microsoft.AspNetCore.Mvc;
using RealTimeChat.DTO.MessageDTOs;
using RealTimeChat.Interface;
using RealTimeChat.Mappers;
using RealTimeChat.Models;

namespace RealTimeChat.Controllers
{
    [Route("api/Messages")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _repo;
        public MessageController(IMessageRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        [Route("{username}")]
        public async Task<IActionResult> GetMessagesByUsername(string username)
        {
            List<MessageDTO> userMessages = await _repo.GetMessagesByUsernameAsync(username);
            return Ok(userMessages);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] CreateMessageDTO createMessageDTO)
        {
            Message messageModel = await _repo.CreateMessageAsync(createMessageDTO);
            return CreatedAtAction(nameof (CreateMessage), messageModel.ToMessageDTO());
        }
        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> DeleteMessage([FromRoute] string Id)
        {
            Message messageModel = await _repo.DeleteMessageAsync(Id);
            return NoContent();
        }
    }
}

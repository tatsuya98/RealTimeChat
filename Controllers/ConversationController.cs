using Microsoft.AspNetCore.Mvc;
using RealTimeChat.DTO.MessageDTOs;
using RealTimeChat.Interface;

namespace RealTimeChat.Controllers
{
    [Route("api/conversation")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationRepository _conversationRepo;

        public ConversationController(IConversationRepository conversationRepo)
        {
            _conversationRepo = conversationRepo;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetConversationHistory([FromRoute] string id)
        {
            List<HistotryMessageDTO>? conversationHistory = await _conversationRepo.GetConversationHistory(id);
            if(conversationHistory == null)
                return NotFound();
            return Ok(conversationHistory);
        }
        [HttpPost]
        public async Task<IActionResult> createConversationHistory([FromBody] CreateMessageDTO createMessageDTO)
        {
            string conversationId = await _conversationRepo.CreateConversationHistory(createMessageDTO);
            return CreatedAtAction(nameof(createConversationHistory), conversationId);
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> storeMessageInConversationHistory([FromRoute] string id, [FromBody] CreateMessageDTO createMessageDTO)
        {
            CreateMessageDTO? message = await _conversationRepo.StoreMessageInConversationHistory(id, createMessageDTO);
            if(message == null)
                return NotFound();
            return NoContent();
        }
    }
}

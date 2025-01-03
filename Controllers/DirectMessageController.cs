using Microsoft.AspNetCore.Mvc;
using RealTimeChat.DTO.MessageDTOs;
using RealTimeChat.Interface;

namespace RealTimeChat.Controllers
{
    [Route("api/DirectMessage")]
    [ApiController]
    public class DirectMessageController : ControllerBase
    {
        private readonly IDirectMessageRepository _directMessageRepo;

        public DirectMessageController(IDirectMessageRepository directMessageRepo)
        {
            _directMessageRepo = directMessageRepo;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> DirectMessageHistory([FromRoute] string id)
        {
            List<HistoryMessageDTO>? directMessageHistory = await _directMessageRepo.GetDirectMessageHistory(id);
            if(directMessageHistory == null)
                return NotFound(new JsonResult(new { message = "Document not found" }));
            return Ok(directMessageHistory);
        }  
    }
}

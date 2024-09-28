using Microsoft.AspNetCore.Mvc;
using RealTimeChat.DTO.UserDTOs;
using RealTimeChat.Interface;
using RealTimeChat.Mappers;
using RealTimeChat.Models;

namespace RealTimeChat.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet]
        [Route("{username}")]
        public async Task<IActionResult> GetUserByUserName([FromRoute]string username)
        {
            User? userModel = await _userRepository.GetUserByUsernameAsync(username);
            if(userModel == null) return NotFound();
            return Ok(userModel.ToUserDto());
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO createUserDTO)
        {
            try
            {
            User? userModel = await _userRepository.CreateUserAsync(createUserDTO);
            return CreatedAtAction(nameof(CreateUser), userModel.ToUserDto());
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("{username}")]
        public async Task<IActionResult> UpdatePassword([FromRoute]string username, [FromBody] UpdateUserDTO updateUserDTO)
        {
            User? userModel = await _userRepository.UpdatePasswordAsync(username, updateUserDTO);
            if(userModel == null) return NotFound();
            return Ok("password has been updated");
        }
    }
}

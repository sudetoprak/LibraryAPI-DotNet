using LibraryManagement.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.Interfaces;

namespace LibraryManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(UserCreateDto userDto)
        {
            var createdUser = await _userService.AddUserAsync(userDto);
            return Ok(createdUser);
        }
    }
}
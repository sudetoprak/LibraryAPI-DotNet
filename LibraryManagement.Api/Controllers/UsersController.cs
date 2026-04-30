using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.DTOs.Requests;
using LibraryManagement.Application.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedResult<UserDto>>> GetUsers(int page = 1, int pageSize = 10)
        {
            var users = await _userService.GetAllUsersAsync(page, pageSize);
            return Ok(users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> PostUser(UserCreateDto userDto)
        {
            var createdUser = await _userService.AddUserAsync(userDto);
            return Ok(createdUser);
        }

        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UserRoleUpdateDto dto)
        {
            var result = await _userService.UpdateUserRoleAsync(id, dto.RoleId);
            if (!result.IsSuccess) return BadRequest(new { error = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}

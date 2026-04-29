using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Claims;
namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            var rentals = await _rentalService.GetAllRentalsAsync(page ,pageSize);
            return Ok(rentals);
        }

        [HttpPost("rent")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Rent([FromBody] RentalCreateDto dto)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;      
            var email = User.FindFirst(ClaimTypes.Email)?.Value;   

            var result = await _rentalService.RentBookAsync(dto.FullName, dto.Email, dto.BookId);

            if (!result.IsSuccess) return BadRequest(new { error = result.Message });
            return Ok(new { message = result.Message });
        }

        [HttpPost("return/{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Return(int id)
        {
            var result = await _rentalService.ReturnBookAsync(id);

            if (!result.IsSuccess)
            {
                if (result.Message.Contains("bulunamadı"))
                {
                    return NotFound(new { error = result.Message });
                }

                return BadRequest(new { error = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}
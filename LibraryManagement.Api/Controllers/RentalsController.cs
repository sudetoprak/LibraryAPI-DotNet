using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application;

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
        public async Task<IActionResult> GetAll()
        {
            var rentals = await _rentalService.GetAllRentalsAsync();
            return Ok(rentals);
        }

        [HttpPost("rent")]
        public async Task<IActionResult> Rent(int userId, int bookId)
        {
            var result = await _rentalService.RentBookAsync(userId, bookId);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPost("return/{id}")]
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
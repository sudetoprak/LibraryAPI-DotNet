using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application; 

namespace LibraryManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpPost("rent")]
        public async Task<IActionResult> RentBook(int userId, int bookId)
        {
            var result = await _rentalService.RentBookAsync(userId, bookId);

            if (result.Contains("başarıyla"))
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetRentals()
        {
            var rentals = await _rentalService.GetAllRentalsAsync();
            return Ok(rentals);
        }

        [HttpDelete("return/{id}")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var result = await _rentalService.ReturnBookAsync(id);

            if (result.Contains("Hata"))
                return NotFound(result);

            return Ok(result);
        }
    }
}
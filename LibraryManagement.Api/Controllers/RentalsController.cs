using LibraryManagement.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Application;

namespace LibraryManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RentalsController : ControllerBase
{
    private readonly IRentalService _rentalService;
    private readonly AppDbContext _context;

    public RentalsController(IRentalService rentalService, AppDbContext context)
    {
        _rentalService = rentalService;
        _context = context;
    }

    [HttpPost("rent")]
    public async Task<IActionResult> RentBook(int userId, int bookId)
    {
        var result = await _rentalService.RentBookAsync(userId, bookId);
        if (result.Contains("başarıyla")) return Ok(result);
        return BadRequest(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetRentals()
    {
        var rentals = await _context.Rentals
            .Include(r => r.User)
            .Include(r => r.Book)
            .Select(r => new
            {
                r.Id,
                r.RentalDate,
                UserName = r.User != null ? r.User.FullName : "Bilinmeyen Kullanıcı",
                BookTitle = r.Book != null ? r.Book.Title : "Bilinmeyen Kitap",
                r.BookId,
                r.UserId
            })
            .ToListAsync();

        return Ok(rentals);
    }

    [HttpDelete("return/{id}")]
    public async Task<IActionResult> ReturnBook(int id)
    {
        var result = await _rentalService.ReturnBookAsync(id);

        if (result.Contains("Hata")) return NotFound(result);
        return Ok(result);
    }
}
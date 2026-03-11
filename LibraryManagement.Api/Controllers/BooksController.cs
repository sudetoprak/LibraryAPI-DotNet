using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Api.Controllers;
namespace LibraryManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> PostBook(BookCreateDto bookDto)
    {
        var newBook = await _bookService.AddBookAsync(bookDto);
        return Ok(newBook);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var success = await _bookService.DeleteBookAsync(id);

        if (!success) return NotFound("Kitap bulunamadı!");

        return Ok(new { message = "Kitap başarıyla silindi (Soft Delete)." });
    }
}
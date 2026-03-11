using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly BookService _bookService; 

    public BooksController(BookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> PostBook(BookCreateDto bookDto)
    {
        var newBook = await _bookService.AddBookAsync(bookDto);
        return Ok(newBook);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var success = await _bookService.DeleteBookAsync(id);

        if (!success) return NotFound("Kitap bulunamadı!");

        return NoContent();
    }
}
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Api.Controllers;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.DTOs.Requests;
using LibraryManagement.Application.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
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

    /*
     Yapan = Sude
        Açıklama = Kitap işlemleri için API controller'ı. CRUD işlemlerini içerir.
     */
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PagedResult<BookDto>>> GetBooks(int page = 1, int pageSize = 10)
    {
        var books = await _bookService.GetAllBooksAsync(page, pageSize);
        return Ok(books);
    }
    /*
     * yapan = Sude
     * açıklama = Yeni bir kitap eklemek için POST endpoint'i. BookCreateDto alır ve eklenen kitabın bilgilerini döner.
     */
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<BookDto>> PostBook(BookCreateDto bookDto)
    {
        var newBook = await _bookService.AddBookAsync(bookDto);
        return Ok(newBook);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]

    public async Task<IActionResult> UpdateBook(int id, BookCreateDto dto)
    {
        var success = await _bookService.UpdateBookAsync(id, dto);
        if (!success) return NotFound("Güncellenecek kitap bulunamadı.");
        return Ok("Kitap başarıyla güncellendi.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var success = await _bookService.DeleteBookAsync(id);

        if (!success) return NotFound("Kitap bulunamadı!");

        return Ok(new { message = "Kitap başarıyla silindi (Soft Delete)." });
    }

}

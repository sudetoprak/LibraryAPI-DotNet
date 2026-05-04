using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Api.Controllers;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.DTOs.Requests;
using LibraryManagement.Application.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.IO;
namespace LibraryManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]



public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IWebHostEnvironment _environment;

    public BooksController(IBookService bookService, IWebHostEnvironment environment)
    {
        _bookService = bookService;
        _environment = environment;
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
    public async Task<ActionResult<BookDto>> PostBook([FromForm] BookCreateDto bookDto)
    {
        if (bookDto.Photo != null)
        {
            if (!bookDto.Photo.ContentType.StartsWith("image/"))
                return BadRequest("Sadece resim dosyası yüklenebilir.");

            if (bookDto.Photo.Length > 2 * 1024 * 1024)
                return BadRequest("Dosya boyutu en fazla 2 MB olabilir.");

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "books");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(bookDto.Photo.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await bookDto.Photo.CopyToAsync(stream);
            }

            bookDto.PhotoUrl = $"/uploads/books/{fileName}";
        }

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

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

    //Kitaplarla ilgili listeleme ekleme ve guncelleme işlemini yönetir.
    //Kitap işlemleri IBookService arayüzü kullanılarak servis katmanında gercekleşir. 
    private readonly IBookService _bookService;


    //Kitap işlemleri sırasında dosya yükleme işlemi için IWebHostEnvironment kullanılır.
    private readonly IWebHostEnvironment _environment;


    
    public BooksController(IBookService bookService, IWebHostEnvironment environment)
    { 
        // Constructor çalıştığında IBookService ve IWebHostEnvironment nesneleri dependency injection ile alınır.
      // Böylece controller içinde hem kitap işlemleri hem de dosya yükleme işlemleri yapılabilir.
        _bookService = bookService;
        _environment = environment;
    }

   

    [HttpGet]
    [Authorize]
    //kitapları listelemek icin kullanılan get endpointi.
    //Authorize sayesinde sadece giriş yapmıs kullanıcılar gorebilir.
    public async Task<ActionResult<PagedResult<BookDto>>> GetBooks(int page = 1, int pageSize = 10, string? search = null)
    {
        //servis katmanına gidilerek  veritabanındakı kitaplar page ve page size ile alınır.
        var books = await _bookService.GetAllBooksAsync(page, pageSize, search);
        return Ok(books);
    }
    

    [HttpPost]
    [Authorize(Roles = "Admin")]
    //sadece admin rolu yapan kullanicilar bu işlemi yapabilir.
    public async Task<ActionResult<BookDto>> PostBook([FromForm] BookCreateDto bookDto)
    {
        if (bookDto.Photo != null)
        {
            if (!bookDto.Photo.ContentType.StartsWith("image/"))
                return BadRequest("Sadece resim dosyası yüklenebilir.");

            if (bookDto.Photo.Length > 2 * 1024 * 1024)
                return BadRequest("Dosya boyutu en fazla 2 MB olabilir.");

            var webRootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadsFolder = Path.Combine(webRootPath, "uploads", "books");

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

    public async Task<IActionResult> UpdateBook(int id, [FromForm] BookCreateDto dto)
    {
        if (dto.Photo != null)
        {
            if (!dto.Photo.ContentType.StartsWith("image/"))
                return BadRequest("Sadece resim dosyası yüklenebilir.");

            if (dto.Photo.Length > 2 * 1024 * 1024)
                return BadRequest("Dosya boyutu en fazla 2 MB olabilir.");

            var webRootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadsFolder = Path.Combine(webRootPath, "uploads", "books");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(dto.Photo.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Photo.CopyToAsync(stream);
            }

            dto.PhotoUrl = $"/uploads/books/{fileName}";
        }

        var success = await _bookService.UpdateBookAsync(id, dto);

        if (!success)
            return NotFound("Güncellenecek kitap bulunamadı.");

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

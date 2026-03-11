using LibraryManagement.Infrastructure.Context;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application;

public class BookService : IBookService
{
    private readonly AppDbContext _context;

    public BookService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookDto>> GetAllBooksAsync()
    {
        // Sadece silinmemiş kitapları ve ID bilgisini içeren DTO'ları döner
        return await _context.Books
            .Where(b => !b.IsDeleted) 
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                StockCount = b.StockCount
            }).ToListAsync();
    }

    public async Task<BookDto> AddBookAsync(BookCreateDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            StockCount = dto.StockCount,
            IsDeleted = false
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            StockCount = book.StockCount
        };
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return false;

        // Hocanın istediği gerçek Soft Delete uygulaması
        book.IsDeleted = true; 
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return true;
    }
}
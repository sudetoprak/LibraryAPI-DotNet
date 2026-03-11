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

    // 1. Tüm Kitapları Getir (Soft Delete olanlar hariç)
    public async Task<List<BookDto>> GetAllBooksAsync()
    {
        return await _context.Books
            .Where(b => !b.IsDeleted)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                ISBN = b.ISBN, 
                StockCount = b.StockCount
            }).ToListAsync();
    }

    // 2. Yeni Kitap Ekle
    public async Task<BookDto> AddBookAsync(BookCreateDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            ISBN = dto.ISBN, 
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
            ISBN = book.ISBN,
            StockCount = book.StockCount
        };
    }

    public async Task<bool> UpdateBookAsync(int id, BookCreateDto dto)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null || book.IsDeleted)
            return false;

        book.Title = dto.Title;
        book.Author = dto.Author;
        book.ISBN = dto.ISBN;
        book.StockCount = dto.StockCount;

        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return true;
    }

    // 4. Kitap Sil (Soft Delete)
    public async Task<bool> DeleteBookAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return false;

        book.IsDeleted = true;
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return true;
    }
}
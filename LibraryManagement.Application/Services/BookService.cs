using LibraryManagement.Infrastructure.Context;
using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.DTOs.Requests;
using LibraryManagement.Application.DTOs.Responses;

namespace LibraryManagement.Application.Services;


public class BookService : IBookService
{
    private readonly AppDbContext _context;

    public BookService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<PagedResult<BookDto>> GetAllBooksAsync(int page, int pageSize, string? search = null)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var query = _context.Books.Where(b => !b.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(b =>
                b.Title.Contains(search) ||
                b.Author.Contains(search) ||
                b.ISBN.Contains(search));
        }

        var totalCount = await query.CountAsync();

        var books = await query
            .OrderBy(b => b.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                PhotoUrl = b.PhotoUrl,
                ISBN = b.ISBN,
                StockCount = b.StockCount
            }).ToListAsync();

        return new PagedResult<BookDto>
        {
            Items = books,
            TotalCount = totalCount,
            TotalSize = (int)Math.Ceiling(totalCount / (double)pageSize),
            Page = page,
            PageSize = pageSize
        };
    }
    public async Task<BookDto> AddBookAsync(BookCreateDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            ISBN = dto.ISBN,
            PhotoUrl = dto.PhotoUrl,
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
            PhotoUrl = book.PhotoUrl,
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
        if (!string.IsNullOrWhiteSpace(dto.PhotoUrl))
        {
            book.PhotoUrl = dto.PhotoUrl;
        }

        book.StockCount = dto.StockCount;

        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return true;
    }

 
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

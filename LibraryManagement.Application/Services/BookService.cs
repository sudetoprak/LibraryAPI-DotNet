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
    //kitapları getirirken silinmiş olanları getirmemek için IsDeleted kontrolü ekledik
    public async Task<PagedResult<BookDto>> GetAllBooksAsync(int page, int pageSize)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var query = _context.Books.Where(b => !b.IsDeleted);
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
    //kitap eklerken IsDeleted alanını false olarak ayarlıyoruz çünkü yeni eklenen kitap silinmiş kabul edilmez
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
    //kitap güncellerken de silinmiş olan kitapları güncellemeye çalışmamak için IsDeleted kontrolü ekledik
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

    //kitap silerken de silinmiş olan kitapları tekrar silmeye çalışmamak için IsDeleted kontrolü ekledik. Ayrıca, gerçek bir silme işlemi yerine, kitabın IsDeleted alanını true yaparak "soft delete" uyguluyoruz. Bu sayede, silinen kitaplar veritabanında kalır ancak kullanıcıya gösterilmez .

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

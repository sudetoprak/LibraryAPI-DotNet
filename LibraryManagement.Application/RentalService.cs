using LibraryManagement.Infrastructure.Context;
using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application;

public class RentalService : IRentalService
{
    private readonly AppDbContext _context;

    public RentalService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResult> RentBookAsync(int userId, int bookId)
    {
        var book = await _context.Books.FindAsync(bookId);
        var user = await _context.Users.FindAsync(userId);

        if (book == null) return ServiceResult.Failure("Kitap bulunamadı!");
        if (user == null) return ServiceResult.Failure("Kullanıcı bulunamadı!");
        if (book.StockCount <= 0) return ServiceResult.Failure("Bu kitap şu an stokta yok!");

        var rental = new Rental
        {
            BookId = bookId,
            UserId = userId,
            RentalDate = DateTime.Now,
            IsReturned = false,
            IsDeleted = false
        };

        book.StockCount--;
        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        return ServiceResult.Success($"Kitap başarıyla kiralandı. Kalan stok: {book.StockCount}");
    }

    public async Task<ServiceResult> ReturnBookAsync(int rentalId)
    {
        var rental = await _context.Rentals
            .Include(r => r.Book)
            .IgnoreQueryFilters() 
            .FirstOrDefaultAsync(r => r.Id == rentalId);

        if (rental == null) return ServiceResult.Failure("Kiralama kaydı bulunamadı.");
        if (rental.IsReturned) return ServiceResult.Failure("Bu kitap zaten iade edilmiş.");

        
        if (rental.Book != null)
        {
            rental.Book.StockCount++; 
        }

        rental.IsReturned = true;
        rental.ReturnDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return ServiceResult.Success("Kitap başarıyla iade edildi ve stok güncellendi.");
    }

    public async Task<IEnumerable<object>> GetAllRentalsAsync()
    {
        return await _context.Rentals
            .Include(r => r.User)
            .Include(r => r.Book)
            .Select(r => new
            {
                r.Id,
                r.RentalDate,
                r.ReturnDate,
                r.IsReturned,
                UserName = r.User != null ? r.User.FullName : "Bilinmeyen Kullanıcı",
                BookTitle = r.Book != null ? r.Book.Title : "Bilinmeyen Kitap",
                r.BookId,
                r.UserId
            })
            .ToListAsync();
    }
}
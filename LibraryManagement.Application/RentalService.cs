using LibraryManagement.Infrastructure.Context;
using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application;

public class RentalService : IRentalService
{
    private readonly AppDbContext _context;

    public RentalService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> RentBookAsync(int userId, int bookId)
    {
        var book = await _context.Books.FindAsync(bookId);
        var user = await _context.Users.FindAsync(userId);

        if (book == null) return "Hata: Kitap bulunamadı!";
        if (user == null) return "Hata: Kullanıcı bulunamadı!";
        if (book.StockCount <= 0) return "Hata: Bu kitap şu an stokta yok!";

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

        return $"Kitap başarıyla kiralandı. Kalan stok: {book.StockCount}";
    }

    public async Task<string> ReturnBookAsync(int rentalId)
    {
        var rental = await _context.Rentals
            .Include(r => r.Book)
            .FirstOrDefaultAsync(r => r.Id == rentalId);

        if (rental == null) return "Hata: Kiralama kaydı bulunamadı.";

        if (rental.IsReturned) return "Hata: Bu kitap zaten iade edilmiş.";

        if (rental.Book != null)
        {
            rental.Book.StockCount++; // Stoka geri ekle
        }

        rental.IsReturned = true;
        rental.ReturnDate = DateTime.Now;

        _context.Rentals.Update(rental);
        await _context.SaveChangesAsync();

        return $"Kitap iade edildi. İade Tarihi: {rental.ReturnDate}. Güncel stok: {rental.Book?.StockCount}";
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
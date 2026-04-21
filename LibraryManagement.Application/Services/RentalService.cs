using LibraryManagement.Infrastructure.Context;
using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.DTOs.Responses;
using static LibraryManagement.Domain.Entities.Rental;
using System;
namespace LibraryManagement.Application.Services;

public class RentalService : IRentalService
{
    private readonly AppDbContext _context;

    public RentalService(AppDbContext context)
    {
        _context = context;
    }

    // BURAYI DÜZELTTİK: Parametreler (string fullName, string email, int bookId) olmalı!
    public async Task<ServiceResult> RentBookAsync(string fullName, string email, int bookId)
    {
        // 1. Kitabı kontrol et
        var book = await _context.Books.FindAsync(bookId);
        if (book == null) return ServiceResult.Failure("Kitap bulunamadı!");
        if (book.StockCount <= 0) return ServiceResult.Failure("Bu kitap şu an stokta yok!");

        // 2. Kullanıcıyı bul veya yoksa otomatik oluştur
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            user = new User
            {
                FullName = fullName,
                Email = email
            };
            _context.Users.Add(user);
            // ID'nin oluşması için önce kullanıcıyı kaydediyoruz
            await _context.SaveChangesAsync();
        }

        // 3. Kiralama kaydını oluştur (Artık user.Id garanti)
        var rental = new Rental
        {
            BookId = bookId,
            UserId = user.Id,
            RentalDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(14), // 2 hafta
            Status = RentalStatus.Active,
            IsDeleted = false
        };

        // 4. Stok düşür ve kaydet
        book.StockCount--;
        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        return ServiceResult.Success($"{user.FullName} için kiralama başarılı! Kalan stok: {book.StockCount}");
    }

    public async Task<ServiceResult> ReturnBookAsync(int rentalId)
    {
        var rental = await _context.Rentals
            .Include(r => r.Book)
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(r => r.Id == rentalId);

        if (rental == null) return ServiceResult.Failure("Kiralama kaydı bulunamadı.");
        if (rental.Status == RentalStatus.Returned)
            return ServiceResult.Failure("Bu kitap zaten iade edilmiş.");

        rental.Status = RentalStatus.Returned;
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
                r.Status,
                UserName = r.User != null ? r.User.FullName : "Bilinmeyen Kullanıcı",
                BookTitle = r.Book != null ? r.Book.Title : "Bilinmeyen Kitap",
                r.BookId,
                r.UserId
            })
            .ToListAsync();
    }
}
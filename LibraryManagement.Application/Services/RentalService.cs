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

    public async Task<ServiceResult> RentBookAsync(string fullName, string email, int bookId)
    {
        var book = await _context.Books.FindAsync(bookId);
        if (book == null) return ServiceResult.Failure("Kitap bulunamadı!");
        if (book.StockCount <= 0) return ServiceResult.Failure("Bu kitap şu an stokta yok!");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            user = new User
            {
                FullName = fullName,
                Email = email
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        var rental = new Rental
        {
            BookId = bookId,
            UserId = user.Id,
            RentalDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(14), 
            Status = RentalStatus.Active,
            IsDeleted = false
        };

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

    public async Task<PagedResult<object>> GetAllRentalsAsync(int page, int pageSize)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var query = _context.Rentals
            .Include(r => r.User)
            .Include(r => r.Book);

        var totalCount = await query.CountAsync();

        var rentals = await query
            .OrderByDescending(r => r.RentalDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
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
            .Cast<object>()
            .ToListAsync();

        return new PagedResult<object>
        {
            Items = rentals,
            TotalCount = totalCount,
            TotalSize = (int)Math.Ceiling(totalCount / (double)pageSize),
            Page = page,
            PageSize = pageSize
        };
    }
}

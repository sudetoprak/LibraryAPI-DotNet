using LibraryManagement.Infrastructure.Context;
using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.DTOs.Responses;
using static LibraryManagement.Domain.Entities.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LibraryManagement.Application.Services;

public class RentalService : IRentalService
{
    private readonly AppDbContext _context;

    public RentalService(AppDbContext context)
    {
        _context = context;
    }
    //Kitap kiralama işlemini yapar; kitabın stok durumunu kontrol eder ve kiralama kaydı oluşturur
    public async Task<ServiceResult> RentBookAsync(string fullName, string email, int bookId)
    {
        var book = await _context.Books.FindAsync(bookId);
        if (book == null) return ServiceResult.Failure("Kitap bulunamadı!");
        if (book.StockCount <= 0) return ServiceResult.Failure("Bu kitap şu an stokta yok!");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            return ServiceResult.Failure("Bu e-posta ile kayıtlı kullanıcı bulunamadı. Önce kayıt olunmalıdır.");
        }

        if (!user.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase))
        {
            return ServiceResult.Failure("Girilen ad soyad kayıtlı kullanıcı bilgisiyle eşleşmiyor.");
        }

        var rental = new Rental
        {
            BookId = bookId,
            UserId = user.Id,
            BorrowerName = user.FullName,
            BorrowerEmail = user.Email,
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
    // Kiralanan kitabın iade işlemini yapar ve kiralama durumunu günceller.
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
        rental.IsReturned = true;
        rental.ReturnDate = DateTime.Now;

        if (rental.Book != null)
        {
            rental.Book.StockCount++;
            _context.Books.Update(rental.Book);
        }

        await _context.SaveChangesAsync();
        return ServiceResult.Success("Kitap başarıyla iade edildi ve stok güncellendi.");
    }

    // Tüm kiralama kayıtlarını sayfalı şekilde getirir.
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
                r.DueDate,
                r.ReturnDate,
                r.IsReturned,
                r.Status,
                UserName = !string.IsNullOrWhiteSpace(r.BorrowerName)
                    ? r.BorrowerName
                    : r.User != null ? r.User.FullName : "Bilinmeyen Kullanıcı",
                BorrowerEmail = !string.IsNullOrWhiteSpace(r.BorrowerEmail)
                    ? r.BorrowerEmail
                    : r.User != null ? r.User.Email : string.Empty,
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
    // Teslim tarihi geçmiş ve iade edilmemiş kiralama kayıtlarını getirir.
    public async Task<PagedResult<object>> GetOverdueRentalsAsync(int page, int pageSize, string? search)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var query = _context.Rentals
            .Include(r => r.User)
            .Include(r => r.Book)
            .Where(r => r.Status != RentalStatus.Returned && r.DueDate < DateTime.Now);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(r =>
                (r.User != null && r.User.FullName.Contains(search)) ||
                (r.Book != null && r.Book.Title.Contains(search)));
        }

        var totalCount = await query.CountAsync();

        var rentals = await query
            .OrderBy(r => r.DueDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new
            {
                r.Id,
                r.RentalDate,
                r.DueDate,
                r.ReturnDate,
                r.IsReturned,
                r.Status,
                UserName = !string.IsNullOrWhiteSpace(r.BorrowerName)
                    ? r.BorrowerName
                    : r.User != null ? r.User.FullName : "Bilinmeyen Kullanıcı",
                BorrowerEmail = !string.IsNullOrWhiteSpace(r.BorrowerEmail)
                    ? r.BorrowerEmail
                    : r.User != null ? r.User.Email : string.Empty,
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
    // Giriş yapan kullanıcının kendi kiralama geçmişini getirir.
    public async Task<PagedResult<object>> GetRentalsByUserAsync(int userId, string email, int page, int pageSize)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var query = _context.Rentals
            .Include(r => r.User)
            .Include(r => r.Book)
            .Where(r => r.UserId == userId || r.BorrowerEmail == email);

        var totalCount = await query.CountAsync();

        var rentals = await query
            .OrderByDescending(r => r.RentalDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new
            {
                r.Id,
                r.RentalDate,
                r.DueDate,
                r.ReturnDate,
                r.IsReturned,
                r.Status,
                UserName = !string.IsNullOrWhiteSpace(r.BorrowerName)
                    ? r.BorrowerName
                    : r.User != null ? r.User.FullName : "Bilinmeyen Kullanıcı",
                BorrowerEmail = !string.IsNullOrWhiteSpace(r.BorrowerEmail)
                    ? r.BorrowerEmail
                    : r.User != null ? r.User.Email : string.Empty,
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

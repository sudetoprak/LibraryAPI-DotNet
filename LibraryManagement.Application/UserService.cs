using LibraryManagement.Infrastructure.Context;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        return await _context.Users
            .Where(u => !u.IsDeleted) 
            .Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email
            })
            .ToListAsync();
    }

    public async Task<UserDto> AddUserAsync(UserCreateDto dto)
    {
        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            IsDeleted = false
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email
        };
    }
}
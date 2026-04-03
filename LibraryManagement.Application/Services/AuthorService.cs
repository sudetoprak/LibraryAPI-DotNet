using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure;
using LibraryManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly AppDbContext _context;

        public AuthorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuthorDto>> GetAllAuthorsAsync()
        {
            return await _context.Authors
                .Where(a => !a.IsDeleted)
                .Select(a => new AuthorDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Bio = a.Bio
                })
                .ToListAsync();
        }
        //yazar eklerken IsDeleted alanını false olarak ayarlıyoruz çünkü yeni eklenen yazar silinmiş kabul edilmez
        public async Task<AuthorDto> AddAuthorAsync(AuthorCreateDto dto)
        {
            var author = new Author
            {
                FullName = dto.FullName,
                Bio = dto.Bio,
                IsDeleted = false
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return new AuthorDto
            {
                Id = author.Id,
                FullName = author.FullName,
                Bio = author.Bio
            };
        }

        public async Task<bool> UpdateAuthorAsync(int id, AuthorCreateDto dto)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null || author.IsDeleted) return false;

            author.FullName = dto.FullName;
            author.Bio = dto.Bio;

            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return false;

            author.IsDeleted = true;
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

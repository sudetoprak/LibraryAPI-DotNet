using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Application.DTOs.Requests;
using LibraryManagement.Application.DTOs.Responses;
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
        
        public async Task<PagedResult<AuthorDto>> GetAllAuthorsAsync(int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var query = _context.Authors.Where(a => !a.IsDeleted);
            var totalCount = await query.CountAsync();

            var authors = await query
                .OrderBy(a => a.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AuthorDto
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Bio = a.Bio
                })
                .ToListAsync();

            return new PagedResult<AuthorDto>
            {
                Items = authors,
                TotalCount = totalCount,
                TotalSize = (int)Math.Ceiling(totalCount / (double)pageSize),
                Page = page,
                PageSize = pageSize
            };
        }
        //yazar eklerken IsDeleted alanını false olarak ayarlıyoruz çünkü yeni eklenen yazar silinmiş kabul edilmez 
        public async Task<AuthorDto> AddAuthorAsync(AuthorCreateDto dto)
        {
            var author = new Author
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Bio = dto.Bio,
                IsDeleted = false
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return new AuthorDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Bio = author.Bio
            };
        }

        public async Task<bool> UpdateAuthorAsync(int id, AuthorCreateDto dto)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null || author.IsDeleted) return false;

            author.FirstName = dto.FirstName;
            author.LastName = dto.LastName;
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

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
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<CategoryDto>> GetAllCategoriesAsync(int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var query = _context.Categories.Where(c => !c.IsDeleted);
            var totalCount = await query.CountAsync();

            var categories = await query
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return new PagedResult<CategoryDto>
            {
                Items = categories,
                TotalCount = totalCount,
                TotalSize = (int)Math.Ceiling(totalCount / (double)pageSize),
                Page = page,
                PageSize = pageSize
            };
        }
        public async Task<CategoryDto> AddCategoryAsync(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                IsDeleted = false
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
        public async Task<bool> UpdateCategoryAsync(int id, CategoryCreateDto dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null || category.IsDeleted) return false;
            category.Name = dto.Name;
            await _context.SaveChangesAsync();
            return true;
        }
       
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null || category.IsDeleted) return false;
            category.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

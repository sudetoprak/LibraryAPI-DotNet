using Microsoft.AspNetCore.Components;
using LibraryManagement.Application.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.DTOs.Requests;
using LibraryManagement.Application.DTOs.Responses;


namespace LibraryManagement.Api.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> PostCategory(CategoryCreateDto dto)
        {
            var category = await _categoryService.AddCategoryAsync(dto);
            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryCreateDto dto)
        {
            var success = await _categoryService.UpdateCategoryAsync(id, dto);
            if (!success) return NotFound("Güncellenecek kategori bulunamadı.");
            return Ok("Kategori başarıyla güncellendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var success = await _categoryService.DeleteCategoryAsync(id);
            if (!success) return NotFound("Kategori bulunamadı!");
            return Ok(new { message = "Kategori başarıyla silindi (Soft Delete)." });
        }

    }
}

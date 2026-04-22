using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Application.DTOs.Requests;
using LibraryManagement.Application.DTOs.Responses;
namespace LibraryManagement.Application.Interfaces
{//interface, servis katmanında hangi işlemlerin yapılacağını belirtiyoruz. Bu sayede, servis katmanında yapılan işlemlerin sonucunu daha kolay yönetebilir ve kullanıcıya anlamlı geri bildirimler sağlayabiliriz.
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> AddCategoryAsync(CategoryCreateDto dto);
        Task<bool> UpdateCategoryAsync(int id, CategoryCreateDto dto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}

using LibraryManagement.Application.DTOs.Requests;
using LibraryManagement.Application.DTOs.Responses;

namespace LibraryManagement.Application.Interfaces
{//Yazar ekleme, güncelleme, silme ve listeleme . (interface )
    public interface IAuthorService
    {
        Task<PagedResult<AuthorDto>> GetAllAuthorsAsync(int page, int pageSize);
        Task<AuthorDto> AddAuthorAsync(AuthorCreateDto dto);
        Task<bool> UpdateAuthorAsync(int id, AuthorCreateDto dto);
        Task<bool> DeleteAuthorAsync(int id);

    }
}

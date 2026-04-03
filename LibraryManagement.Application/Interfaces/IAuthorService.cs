
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<List<AuthorDto>> GetAllAuthorsAsync();
        Task<AuthorDto> AddAuthorAsync(AuthorCreateDto dto);
        Task<bool> UpdateAuthorAsync(int id, AuthorCreateDto dto);
        Task<bool> DeleteAuthorAsync(int id);

    }
}

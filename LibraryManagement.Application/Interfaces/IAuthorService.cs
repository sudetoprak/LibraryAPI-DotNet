using LibraryManagement.Application.DTOs.Requests;
using LibraryManagement.Application.DTOs.Responses;

namespace LibraryManagement.Application.Interfaces
{//Bu interface, yazarlarla ilgili işlemleri tanımlamak için kullanılır. Yazar ekleme, güncelleme, silme ve listeleme gibi işlemleri içerebilir. Bu sayede, servis katmanında yapılan işlemlerin sonucunu daha kolay yönetebilir ve kullanıcıya anlamlı geri bildirimler sağlayabiliriz.
    public interface IAuthorService
    {
        Task<List<AuthorDto>> GetAllAuthorsAsync();
        Task<AuthorDto> AddAuthorAsync(AuthorCreateDto dto);
        Task<bool> UpdateAuthorAsync(int id, AuthorCreateDto dto);
        Task<bool> DeleteAuthorAsync(int id);

    }
}

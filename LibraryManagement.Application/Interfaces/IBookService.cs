using LibraryManagement.Application.DTOs.Requests;
using LibraryManagement.Application.DTOs.Responses;
using LibraryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Interfaces
{
    //interface, servis katmanında hangi işlemlerin yapılacağını belirtiyoruz. Bu sayede, servis katmanında yapılan işlemlerin sonucunu daha kolay yönetebilir ve kullanıcıya anlamlı geri bildirimler sağlayabiliriz.
    
    public interface IBookService
    {
        Task<List<BookDto>> GetAllBooksAsync();
        Task<BookDto> AddBookAsync(BookCreateDto dto);
        Task<bool> DeleteBookAsync(int id);
        Task<bool> UpdateBookAsync(int id, BookCreateDto dto);
    }
}

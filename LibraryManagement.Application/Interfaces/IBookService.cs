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
    // servis katmanında hangi işlemlerin yapılacağını belirtiyoruz. 
    
    public interface IBookService
    {
        Task<PagedResult<BookDto>> GetAllBooksAsync(int page, int pagesize, string? search = null);
        Task<BookDto> AddBookAsync(BookCreateDto dto);
        Task<bool> DeleteBookAsync(int id);
        Task<bool> UpdateBookAsync(int id, BookCreateDto dto);
    }
}

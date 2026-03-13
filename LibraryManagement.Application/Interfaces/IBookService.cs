using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Interfaces
{
    public interface IBookService
    {
        Task<List<BookDto>> GetAllBooksAsync();
        Task<BookDto> AddBookAsync(BookCreateDto dto);
        Task<bool> DeleteBookAsync(int id);
        Task<bool> UpdateBookAsync(int id, BookCreateDto dto);
    }
}

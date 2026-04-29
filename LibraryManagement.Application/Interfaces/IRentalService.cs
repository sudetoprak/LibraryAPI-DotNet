using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Application.DTOs.Responses;
using LibraryManagement.Application.DTOs.Requests;
namespace LibraryManagement.Application.Interfaces;


public interface IRentalService
{
    
    Task<ServiceResult> RentBookAsync(string fullName, string email, int bookId);

    Task<ServiceResult> ReturnBookAsync(int rentalId);
    Task<PagedResult<object>> GetAllRentalsAsync(int page,int pageSize);
}
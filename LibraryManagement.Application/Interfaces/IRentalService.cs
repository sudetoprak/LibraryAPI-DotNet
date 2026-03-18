using LibraryManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Interfaces;

public interface IRentalService
{
   
    Task<ServiceResult> RentBookAsync(string fullName, string email, int bookId);

    Task<ServiceResult> ReturnBookAsync(int rentalId);
    Task<IEnumerable<object>> GetAllRentalsAsync();
}
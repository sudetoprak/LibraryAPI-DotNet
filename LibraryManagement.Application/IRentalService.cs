using LibraryManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application
{
    public interface IRentalService
    {
        Task<ServiceResult> RentBookAsync(int userId, int bookId);
        Task<ServiceResult> ReturnBookAsync(int rentalId);
        Task<IEnumerable<object>> GetAllRentalsAsync();
    }
}

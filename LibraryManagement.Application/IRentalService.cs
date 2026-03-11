using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application
{
    public interface IRentalService
    {
        Task<string> RentBookAsync(int userId, int bookId);
        Task<string> ReturnBookAsync(int rentalId);
        Task<IEnumerable<object>> GetAllRentalsAsync(); 
    }
}

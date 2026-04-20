using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Application.DTOs.Responses;

namespace LibraryManagement.Application.Interfaces;


public interface IRentalService
{
    // Bu interface, kitap kiralama işlemlerini yönetmek için kullanılan bir arayüzdür. Kitap kiralama işlemi için RentBookAsync yöntemi, kitap iade işlemi için ise ReturnBookAsync yöntemi bulunmaktadır. Ayrıca, tüm kiralamaları listelemek için GetAllRentalsAsync yöntemi de tanımlanmıştır. Bu sayede, uygulamanın farklı bölümlerinde kitap kiralama işlemleri merkezi bir şekilde yönetilebilir ve kullanıcıya anlamlı geri bildirimler sağlanabilir.
    Task<ServiceResult> RentBookAsync(string fullName, string email, int bookId);

    Task<ServiceResult> ReturnBookAsync(int rentalId);
    Task<IEnumerable<object>> GetAllRentalsAsync();
}
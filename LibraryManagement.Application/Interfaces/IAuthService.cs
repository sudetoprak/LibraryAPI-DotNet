using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Application.DTOs.Responses;

namespace LibraryManagement.Application.Interfaces

{// kayıt ve giriş işlemlerini yönetmek için kullanılan bir arayüzdür.
    public interface IAuthService
    {
        Task<ServiceResult>RegisterAsync(string fullName, string email, string password);
        Task<string>LoginAsync(LoginDto dto);
    }
}

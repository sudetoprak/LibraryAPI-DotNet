using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Interfaces

{
    public interface IAuthService
    {
        Task<ServiceResult>RegisterAsync(string fullName, string email, string password);
        Task<string>LoginAsync(LoginDto dto);
    }
}

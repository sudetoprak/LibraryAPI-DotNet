using LibraryManagement.Application.DTOs.Requests;
using LibraryManagement.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Interfaces
{// IUserService, kullanıcılarla ilgili işlemleri tanımlayan bir arayüzdür. Bu arayüz, kullanıcıların listelenmesi ve yeni kullanıcı eklenmesi gibi işlemleri içermektedir. Bu sayede, kullanıcı yönetimi işlemlerini merkezi bir şekilde yönetebilir ve uygulamanın diğer bölümlerinde bu işlemleri kolayca kullanabiliriz.
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> AddUserAsync(UserCreateDto dto);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Application.DTOs.Responses;

namespace LibraryManagement.Application.Interfaces

{// IAuthService, kullanıcıların kayıt ve giriş işlemlerini yönetmek için kullanılan bir arayüzdür. Bu arayüz, kullanıcıların sisteme kaydolmasını ve giriş yapmasını sağlayacak yöntemleri tanımlar. Kayıt işlemi için RegisterAsync yöntemi, giriş işlemi için ise LoginAsync yöntemi bulunmaktadır. Bu sayede, uygulamanın farklı bölümlerinde kullanıcı kimlik doğrulama işlemleri merkezi bir şekilde yönetilebilir ve güvenli bir şekilde gerçekleştirilebilir.
    public interface IAuthService
    {
        Task<ServiceResult>RegisterAsync(string fullName, string email, string password);
        Task<string>LoginAsync(LoginDto dto);
    }
}

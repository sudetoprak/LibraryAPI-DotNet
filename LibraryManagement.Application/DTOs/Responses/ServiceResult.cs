using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Responses
{
    // servis işlemlerinin sonucunu temsil eden bir sınıf oluşturuyoruz. Bu sınıf, işlemin başarılı olup olmadığını ve varsa bir mesajı içerecek şekilde tasarlanmıştır. Bu sayede, servis katmanında yapılan işlemlerin sonucunu daha kolay yönetebilir ve kullanıcıya anlamlı geri bildirimler sağlayabiliriz.
    public  class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public static ServiceResult Success(string message = "") => new() { IsSuccess = true, Message = message };
        public static ServiceResult Failure(string message) => new() { IsSuccess = false, Message = message };
    }
}

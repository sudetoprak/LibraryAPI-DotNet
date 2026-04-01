using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs
{
    //kullanıc tarafından yeni bir kullanıcı oluşturmak istediğinde ,kullanıcıdan hangi bilgileri alacağımızı belirtiyoruz
    //fullname ve email 
    public class UserCreateDto
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta formatı.")]
        public string Email { get; set; } = string.Empty;
    }
}

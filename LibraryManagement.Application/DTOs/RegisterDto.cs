using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace LibraryManagement.Application.DTOs
    
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Ad soyad zorunludur.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parola zorunludur.")]
        [MinLength(6, ErrorMessage = "Parola en az 6 karakter olmalıdır.")]
        public string Password { get; set; } = string.Empty;

    }
}

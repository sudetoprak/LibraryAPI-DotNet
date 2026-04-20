using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace LibraryManagement.Application.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email zorunludur.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Parola zorunludur.")]
        public string Password { get; set; } = string.Empty;
    }
}

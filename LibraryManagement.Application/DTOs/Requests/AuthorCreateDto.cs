using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Requests
{//Bu sınıf, yeni bir yazar eklemek için kullanılan DTO'dur. Kullanıcıdan yazarın tam adını ve biyografisini alır. FullName alanı zorunludur ve boş bırakılamaz.
    public class AuthorCreateDto
    {
        [Required(ErrorMessage = "Yazar adi zorunludur.")]
        public string FullName { get; set; }
        public string Bio { get; set; }
    }
}

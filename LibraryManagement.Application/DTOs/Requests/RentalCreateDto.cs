using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Requests
{// kitap kiralama işlemi için kullanıcıdan hangi bilgileri alacağımızı belirtiyoruz.
 //bookid ,fullname ve email bilgilerini alıyoruz çünkü kiralama işlemi için hangi kitabın kiralanacağını ,kimin kiralayacağını ve iletişim bilgilerini bilmemiz gerekiyor

    public class RentalCreateDto
    {
        public int BookId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
    
}


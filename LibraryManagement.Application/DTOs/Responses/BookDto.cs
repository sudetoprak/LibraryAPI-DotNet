using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Responses
{
    //Dto ,kullanıcıcn ham verisini deil ,sadece kullanıcının görmek istediği kısımdır 
    public class BookDto
    {
        //Book sınıfından sadece kullanıcıya göstermek istediğimiz alanları seçiyoruz
        //is delete ve isavailable gibi alanları göstermiyoruz çünkü kullanıcıya sadece kitap bilgilerini göstermek istiyoruz
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int StockCount { get; set; }
        public string ISBN { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain
{
    public class BaseEntity
    {  //sınıfların kalıtım ile alacagı sınıf
        public int Id { get; set; }
        public bool IsDeleted { get; set; } =false;

        //Kaydın olusturuldugu tarih
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //kayıt sonradan guncellenırse guncelleme tarihini tutar 
        public DateTime? UpdatedAt { get; set; }

    }
}

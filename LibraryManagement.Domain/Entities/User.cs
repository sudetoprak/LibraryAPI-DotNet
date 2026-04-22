using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.Entities;
namespace LibraryManagement.Domain.Entities
{
    public class User:BaseEntity
    { // User sınıfı, kütüphane yönetim sistemindeki kullanıcıları temsil eder. Bu sınıf, kullanıcıların kimlik bilgilerini, iletişim bilgilerini ve rollerini içeren özelliklere sahiptir. User sınıfı, kullanıcıların sisteme giriş yapabilmesi, kitap kiralayabilmesi ve diğer işlemleri gerçekleştirebilmesi için gerekli bilgileri tutar. Ayrıca, kullanıcıların sahip olduğu roller aracılığıyla sistemdeki yetkilerini belirlemek için Role sınıfıyla ilişkilidir.
        public string PasswordHash { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public ICollection<Rental>? Rentals { get; set; } = new List<Rental>();
    }
}


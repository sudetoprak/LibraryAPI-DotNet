using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain;


namespace LibraryManagement.Domain.Entities
{// Role sınıfı, kullanıcıların sahip olabileceği farklı rollerin tanımlandığı bir varlık sınıfıdır. Bu sınıf, kullanıcıların sistemdeki yetkilerini ve erişim seviyelerini belirlemek için kullanılır. Örneğin, bir kullanıcı "Admin" rolüne sahip olabilir ve bu rol, yönetim paneline erişim gibi özel izinler içerebilir. Role sınıfı, kullanıcılarla bire çok ilişki içindedir, yani bir rol birden fazla kullanıcıya atanabilir.
    public class Role: BaseEntity
    {
        public string Name { get; set; }=string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = new List<User>();

    }
}
